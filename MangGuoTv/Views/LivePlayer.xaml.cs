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
using System.Windows.Threading;

namespace MangGuoTv.Views
{

    public partial class LivePlayer : PhoneApplicationPage
    {
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

        //static readonly TimeSpan StepSize = TimeSpan.FromMinutes(2);
        //static readonly IApplicationInformation ApplicationInformation = ApplicationInformationFactory.Default;
        //readonly IMediaElementManager _mediaElementManager;
        //readonly DispatcherTimer _positionSampler;
        //IMediaStreamFascade _mediaStreamFascade;
        //TimeSpan _previousPosition;
        //readonly IHttpClients _httpClients;
        public LivePlayer()
        {
            InitializeComponent();

            //_mediaElementManager = new MediaElementManager(Dispatcher,
            //    () =>
            //    {
            //        UpdateState(MediaElementState.Opening);

            //        return livePlayer;
            //    },
            //    me => UpdateState(MediaElementState.Closed));

            //_httpClients = new HttpClients(userAgent: ApplicationInformation.CreateUserAgent());

            //_positionSampler = new DispatcherTimer
            //{
            //    Interval = TimeSpan.FromMilliseconds(75)
            //};
            //_positionSampler.Tick += OnPositionSamplerOnTick;

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
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //InitializeMediaStream();

            //_mediaStreamFascade.Source = new Uri(liveUrl);

            //livePlayer.Play();

            //_positionSampler.Start();
        }
        //void mediaElement1_CurrentStateChanged(object sender, RoutedEventArgs e)
        //{
        //    var state = null == livePlayer ? MediaElementState.Closed : livePlayer.CurrentState;

        //    if (null != _mediaStreamFascade)
        //    {
        //        var managerState = _mediaStreamFascade.State;

        //        if (MediaElementState.Closed == state)
        //        {
        //            if (TsMediaManager.MediaState.OpenMedia == managerState || TsMediaManager.MediaState.Opening == managerState || TsMediaManager.MediaState.Playing == managerState)
        //                state = MediaElementState.Opening;
        //        }
        //    }

        //    UpdateState(state);
        //}

        //void UpdateState(MediaElementState state)
        //{
        //    Debug.WriteLine("MediaElement State: " + state);

        //    if (MediaElementState.Buffering == state && null != livePlayer)
        //        //MediaStateBox.Text = string.Format("Buffering {0:F2}%", livePlayer.BufferingProgress * 100);
        //    ////else
        //       // MediaStateBox.Text = state.ToString();

        //    switch (state)
        //    {
        //        case MediaElementState.Closed:
        //           // playButton.IsEnabled = true;
        //           // stopButton.IsEnabled = false;
        //            break;
        //        case MediaElementState.Paused:
        //           // playButton.IsEnabled = true;
        //           // stopButton.IsEnabled = true;
        //           // errorBox.Visibility = Visibility.Collapsed;
        //            break;
        //        case MediaElementState.Playing:
        //            //playButton.IsEnabled = false;
        //            //stopButton.IsEnabled = true;
        //            //errorBox.Visibility = Visibility.Collapsed;
        //            break;
        //        default:
        //            //stopButton.IsEnabled = true;
        //            //errorBox.Visibility = Visibility.Collapsed;
        //            break;
        //    }

        //    OnPositionSamplerOnTick(null, null);
        //}

        //void OnPositionSamplerOnTick(object o, EventArgs ea)
        //{
        //    if (null == livePlayer || (MediaElementState.Playing != livePlayer.CurrentState && MediaElementState.Paused != livePlayer.CurrentState))
        //    {
        //        //PositionBox.Text = "--:--:--.--";

        //        return;
        //    }

        //    var positionSample = livePlayer.Position;

        //    if (positionSample == _previousPosition)
        //        return;

        //    _previousPosition = positionSample;

        //    //PositionBox.Text = FormatTimeSpan(positionSample);
        //}

        //string FormatTimeSpan(TimeSpan timeSpan)
        //{
        //    var sb = new StringBuilder();

        //    if (timeSpan < TimeSpan.Zero)
        //    {
        //        sb.Append('-');

        //        timeSpan = -timeSpan;
        //    }

        //    if (timeSpan.Days > 1)
        //        sb.AppendFormat(timeSpan.ToString(@"%d\."));

        //    sb.Append(timeSpan.ToString(@"hh\:mm\:ss\.ff"));

        //    return sb.ToString();
        //}

        //void play_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Play clicked");

        //    if (null == livePlayer)
        //    {
        //        Debug.WriteLine("MainPage Play no media element");
        //        return;
        //    }

        //    if (MediaElementState.Paused == livePlayer.CurrentState)
        //    {
        //        livePlayer.Play();
        //        return;
        //    }

        //    //errorBox.Visibility = Visibility.Collapsed;
        //    //playButton.IsEnabled = false;

        //    InitializeMediaStream();

        //    _mediaStreamFascade.Source = new Uri(
        //        //"http://www.nasa.gov/multimedia/nasatv/NTV-Public-IPS.m3u8"
        //        //"http://devimages.apple.com/iphone/samples/bipbop/bipbopall.m3u8"
        //        //"https://devimages.apple.com.edgekey.net/streaming/examples/bipbop_16x9/bipbop_16x9_variant.m3u8"
        //        //"http://pchlsws4.imgo.tv/c811af7cf937bab31c387e51699d80de/55542891/c1/2015/dianshiju/liaozhaixinbianshouluban/201505136a637225-f2c8-49a2-919d-62f0e73a2233.fhv/playlist.m3u8"
        //        "http://3glivehntv.doplive.com.cn/video4/index_128k.m3u8?date=20150518174518&uid=&rnd=2015051817451868760&client=imgo&key=846526a7c999a71acb95e28116d3ace2&cip=54.223.151.189&cck=390f2d1a809cdf7cf9f3c8e33fcf0223"
        //        );

        //    livePlayer.Play();

        //    _positionSampler.Start();
        //}

        //void InitializeMediaStream()
        //{
        //    if (null != _mediaStreamFascade)
        //        return;

        //    _mediaStreamFascade = MediaStreamFascadeSettings.Parameters.Create(_httpClients, _mediaElementManager.SetSourceAsync);

        //    _mediaStreamFascade.SetParameter(_mediaElementManager);

        //    _mediaStreamFascade.StateChange += TsMediaManagerOnStateChange;
        //}

        //void StopMedia()
        //{
        //    _positionSampler.Stop();

        //    if (null != livePlayer)
        //        livePlayer.Source = null;
        //}

        //void CloseMedia()
        //{
        //    StopMedia();

        //    if (null == _mediaStreamFascade)
        //        return;

        //    var mediaStreamFascade = _mediaStreamFascade;

        //    _mediaStreamFascade = null;

        //    mediaStreamFascade.StateChange -= TsMediaManagerOnStateChange;

        //    // Don't block the cleanup in case someone is mashing the play button.
        //    // It could deadlock.
        //    mediaStreamFascade.DisposeBackground("MainPage CloseMedia");
        //}

        //void TsMediaManagerOnStateChange(object sender, TsMediaManagerStateEventArgs tsMediaManagerStateEventArgs)
        //{
        //    Dispatcher.BeginInvoke(() =>
        //    {
        //        var message = tsMediaManagerStateEventArgs.Message;

        //        if (!string.IsNullOrWhiteSpace(message))
        //        {
        //            //errorBox.Text = message;
        //            //errorBox.Visibility = Visibility.Visible;
        //        }

        //        mediaElement1_CurrentStateChanged(null, null);
        //    });
        //}

        //void mediaElement1_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        //{
        //    //errorBox.Text = e.ErrorException.Message;
        //    //errorBox.Visibility = Visibility.Visible;

        //    CloseMedia();

        //    //playButton.IsEnabled = true;
        //}

        //void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("MainPage MediaEnded");

        //    StopMedia();
        //}

        //void stopButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Stop clicked");

        //    StopMedia();
        //}

        //void wakeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Wake clicked");

        //    if (Debugger.IsAttached)
        //        Debugger.Break();

        //    mediaElement1_CurrentStateChanged(null, null);
        //}

        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    base.OnNavigatedFrom(e);

        //    CloseMedia();

        //    if (null != _mediaElementManager)
        //    {
        //        _mediaElementManager.CloseAsync()
        //                            .Wait();
        //    }
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    CloseMedia();

        //    if (null != _mediaElementManager)
        //    {
        //        _mediaElementManager.CloseAsync()
        //                            .Wait();
        //    }
        //}

        //void plusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (null == livePlayer || livePlayer.CurrentState != MediaElementState.Playing)
        //        return;

        //    var position = livePlayer.Position;

        //    livePlayer.Position = position + StepSize;

        //    Debug.WriteLine("Step from {0} to {1} (CanSeek: {2} NaturalDuration: {3})", position, livePlayer.Position, livePlayer.CanSeek, livePlayer.NaturalDuration);
        //}

        //void minusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (null == livePlayer || livePlayer.CurrentState != MediaElementState.Playing)
        //        return;

        //    var position = livePlayer.Position;

        //    if (position < StepSize)
        //        position = TimeSpan.Zero;
        //    else
        //        position -= StepSize;

        //    livePlayer.Position = position;

        //    Debug.WriteLine("Step from {0} to {1} (CanSeek: {2} NaturalDuration: {3})", position, livePlayer.Position, livePlayer.CanSeek, livePlayer.NaturalDuration);
        //}

        //void mediaElement1_BufferingProgressChanged(object sender, RoutedEventArgs e)
        //{
        //    mediaElement1_CurrentStateChanged(sender, e);
        //}


    }
}