using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows.Media;
using SM.Media;
using SM.Media.Utility;
using SM.Media.Web;
using System.Windows.Threading;
using System.Text;
using System.Windows.Media.Imaging;

namespace MangGuoTv.Views
{

    public partial class LivePlayer : PhoneApplicationPage
    {
        private string pauseImg = "/Images/pause.png";
        private string playImg = "/Images/start.png";
        private bool playImgStatus = false;
        public static string liveUrl { get; set; }
#if STREAM_SWITCHING
        static readonly string[] Sources =
        {
            "http://www.npr.org/streams/mp3/nprlive24.pls",
            "http://www.nasa.gov/multimedia/nasatv/NTV-Public-IPS.m3u8",
            "http://devimages.apple.com/iphone/samples/bipbop/bipbopall.m3u8",
            null,
            "https://devimages.apple.com.edgekey.net/streaming/examples/bipbop_16x9/bipbop_16x9_variant.m3u8"
        };

        readonly DispatcherTimer _timer;
        int _count;
#endif

        static readonly TimeSpan StepSize = TimeSpan.FromMinutes(2);
        static readonly IApplicationInformation ApplicationInformation = ApplicationInformationFactory.Default;
        readonly IMediaElementManager _mediaElementManager;
        readonly DispatcherTimer _positionSampler;
        IMediaStreamFascade _mediaStreamFascade;
        TimeSpan _previousPosition;
        readonly IHttpClients _httpClients;

        // Constructor
        public LivePlayer()
        {
            InitializeComponent();

            _mediaElementManager = new MediaElementManager(Dispatcher,
                () =>
                {
                    UpdateState(MediaElementState.Opening);

                    return mediaElement1;
                },
                me => UpdateState(MediaElementState.Closed));

            _httpClients = new HttpClients(userAgent: ApplicationInformation.CreateUserAgent());

            _positionSampler = new DispatcherTimer
                               {
                                   Interval = TimeSpan.FromMilliseconds(75)
                               };
            _positionSampler.Tick += OnPositionSamplerOnTick;

#if STREAM_SWITCHING
            _timer = new DispatcherTimer();

            _timer.Tick += (sender, args) =>
                           {
                               GC.Collect();
                               GC.WaitForPendingFinalizers();
                               GC.Collect();

                               var gcMemory = GC.GetTotalMemory(true).BytesToMiB();

                               var source = Sources[_count];

                               Debug.WriteLine("Switching to {0} (GC {1:F3} MiB App {2:F3}/{3:F3}/{4:F3} MiB)", source, gcMemory,
                                   DeviceStatus.ApplicationCurrentMemoryUsage.BytesToMiB(),
                                   DeviceStatus.ApplicationPeakMemoryUsage.BytesToMiB(),
                                   DeviceStatus.ApplicationMemoryUsageLimit.BytesToMiB());

                               InitializeMediaStream();

                               _mediaStreamFascade.Source = null == source ? null : new Uri(source);

                               if (++_count >= Sources.Length)
                                   _count = 0;

                               _positionSampler.Start();
                           };

            _timer.Interval = TimeSpan.FromSeconds(15);

            _timer.Start();
#endif // STREAM_SWITCHING
        }
        private void mediaElement1_Loaded(object sender, RoutedEventArgs e)
        {

            InitializeMediaStream();

            _mediaStreamFascade.Source = new Uri(liveUrl);

            mediaElement1.Play();
            _positionSampler.Start();

        }

        void mediaElement1_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            var state = null == mediaElement1 ? MediaElementState.Closed : mediaElement1.CurrentState;

            if (null != _mediaStreamFascade)
            {
                var managerState = _mediaStreamFascade.State;

                if (MediaElementState.Closed == state)
                {
                    if (TsMediaManager.MediaState.OpenMedia == managerState || TsMediaManager.MediaState.Opening == managerState || TsMediaManager.MediaState.Playing == managerState)
                        state = MediaElementState.Opening;
                }
            }

            UpdateState(state);
        }

        void UpdateState(MediaElementState state)
        {
            Debug.WriteLine("MediaElement State: " + state);

            //if (MediaElementState.Buffering == state && null != mediaElement1)
                //MediaStateBox.Text = string.Format("Buffering {0:F2}%", mediaElement1.BufferingProgress * 100);
            //else
               // MediaStateBox.Text = state.ToString();

            switch (state)
            {
                case MediaElementState.Closed:
                    PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
                    break;
                case MediaElementState.Paused:
                    PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
                    break;
                case MediaElementState.Playing:
                    bufferBor.Visibility = System.Windows.Visibility.Collapsed;
                    PlayImg.Source = new BitmapImage(new Uri(pauseImg, UriKind.RelativeOrAbsolute));
                    break;
                default:
                    break;
            }

            OnPositionSamplerOnTick(null, null);
        }

        void OnPositionSamplerOnTick(object o, EventArgs ea)
        {
            if (null == mediaElement1 || (MediaElementState.Playing != mediaElement1.CurrentState && MediaElementState.Paused != mediaElement1.CurrentState))
            {
                return;
            }

            var positionSample = mediaElement1.Position;

            if (positionSample == _previousPosition)
                return;

            _previousPosition = positionSample;
        }

        string FormatTimeSpan(TimeSpan timeSpan)
        {
            var sb = new StringBuilder();

            if (timeSpan < TimeSpan.Zero)
            {
                sb.Append('-');

                timeSpan = -timeSpan;
            }

            if (timeSpan.Days > 1)
                sb.AppendFormat(timeSpan.ToString(@"%d\."));

            sb.Append(timeSpan.ToString(@"hh\:mm\:ss\.ff"));

            return sb.ToString();
        }

      

        void InitializeMediaStream()
        {
            if (null != _mediaStreamFascade)
                return;

            _mediaStreamFascade = MediaStreamFascadeSettings.Parameters.Create(_httpClients, _mediaElementManager.SetSourceAsync);

            _mediaStreamFascade.SetParameter(_mediaElementManager);

            _mediaStreamFascade.StateChange += TsMediaManagerOnStateChange;
        }

        void StopMedia()
        {
            _positionSampler.Stop();

            if (null != mediaElement1)
                mediaElement1.Source = null;
        }

        void CloseMedia()
        {
            StopMedia();

            if (null == _mediaStreamFascade)
                return;

            var mediaStreamFascade = _mediaStreamFascade;

            _mediaStreamFascade = null;

            mediaStreamFascade.StateChange -= TsMediaManagerOnStateChange;

            // Don't block the cleanup in case someone is mashing the play button.
            // It could deadlock.
            mediaStreamFascade.DisposeBackground("MainPage CloseMedia");
        }

        void TsMediaManagerOnStateChange(object sender, TsMediaManagerStateEventArgs tsMediaManagerStateEventArgs)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       var message = tsMediaManagerStateEventArgs.Message;

                                       if (!string.IsNullOrWhiteSpace(message))
                                       {
                                           System.Diagnostics.Debug.WriteLine(message);
                                       }

                                       mediaElement1_CurrentStateChanged(null, null);
                                   });
        }

        void mediaElement1_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine(" mediaElement1_MediaFailed");

            CloseMedia();
        }

        void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(" MediaEnded");

            StopMedia();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            CloseMedia();

            if (null != _mediaElementManager)
            {
                _mediaElementManager.CloseAsync()
                                    .Wait();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CloseMedia();

            if (null != _mediaElementManager)
            {
                _mediaElementManager.CloseAsync()
                                    .Wait();
            }
        }

        void plusButton_Click(object sender, RoutedEventArgs e)
        {
            if (null == mediaElement1 || mediaElement1.CurrentState != MediaElementState.Playing)
                return;

            var position = mediaElement1.Position;

            mediaElement1.Position = position + StepSize;

            Debug.WriteLine("Step from {0} to {1} (CanSeek: {2} NaturalDuration: {3})", position, mediaElement1.Position, mediaElement1.CanSeek, mediaElement1.NaturalDuration);
        }

       

        void mediaElement1_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            bufferBor.Visibility = System.Windows.Visibility.Visible;
            mediaElement1_CurrentStateChanged(sender, e);
        }

        private void PlayerGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            playImgStatus = !playImgStatus;
            if (playImgStatus)
            {
                mediaElement1.Play();
                PlayImg.Source = new BitmapImage(new Uri(pauseImg, UriKind.RelativeOrAbsolute));

            }
            else
            {
                mediaElement1.Pause();
                PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
            }
        }

        private void mediaElement1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            playControl.Visibility = (playControl.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

       
    }
}
