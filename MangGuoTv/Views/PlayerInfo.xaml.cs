using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.Models;
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Windows.Media;
using System.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using MangGuoTv.PopUp;

namespace MangGuoTv
{
    public partial class PlayerInfo : PhoneApplicationPage
    {
        private bool IsDownMode = false;
        private string pauseImg = "/Images/pause.png";
        private string needSetSliderValueIso = "needSetSliderValue";
        private string leaveSilderValueIso = "leaveSilderValue";
        private string playImg = "/Images/start.png";
        private bool playImgStatus = false;
        private double leaveSilderValue = 0;
        public static bool needSetSliderValue
        {
            get;
            set;
        }
        public PlayerInfo()
        {
            InitializeComponent();
            LoadDownIcon();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.PlayerModel;
            LoadDramaSeletedItem(App.PlayerModel.VideoId);
            if (App.PlayerModel.MediaSource != null) 
            {
                App.PlayerModel.MediaSource = App.PlayerModel.MediaSource;
            }
            App.ShowLoading();
            App.PlayerModel.LoadVisibility = Visibility.Visible;
            App.PlayerModel.PayVisibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            currentPosition.Stop();
            //当用户按win键 或者长按返回键时  不清空 datacontext  否则从墓碑模式返回时会丢失当前数据
            if (e.Content != null)
            {
                leaveSilderValue = pbVideo.Value;
                App.PlayerModel.ClearData();

                this.DataContext = null;
            }
            else
            {
                needSetSliderValue = true;
            }
            App.HideLoading();
            WpStorage.SetIsoSetting(needSetSliderValueIso, needSetSliderValue);
            WpStorage.SetIsoSetting(leaveSilderValueIso, leaveSilderValue);
        }

        private void DramaItem_Loaded(object sender, RoutedEventArgs e)
        {
            App.PlayerModel.LoadedDramaItem();
        }
        /// <summary>
        /// 判断当前要选中哪一个video
        /// </summary>
        /// <param name="p"></param>
        public void LoadDramaSeletedItem(string videoId,bool fromDown = false)
        {
            int index = 0;
            //如果是切换下载模式 默认置为 -1
            if (fromDown) index = -1;
            for (int i = 0; i < AllDramas.Items.Count; i++) 
            {
                VideoInfo info = AllDramas.Items[i] as VideoInfo;
                if (info != null && info.videoId == videoId) 
                {
                    index = i;
                    break;
                } 
            }
            if (AllDramas.Items.Count > 0) 
            {
                AllDramas.SelectedIndex = index;
                AllDramas.ScrollIntoView(AllDramas.SelectedItem);
            }
        }
        private int currentDramaIndex = -1;
        private void AllDramas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllDramas.SelectedIndex == -1)
            {
                if (IsDownMode) startDownBtn.IsEnabled = false;
                return;
            } 
            if (IsDownMode) 
            {
                if (AllDramas.SelectedItems.Count == 0)
                {
                    startDownBtn.IsEnabled = false;
                } 
                else if(AllDramas.SelectedItems.Count > 0) 
                {
                    startDownBtn.IsEnabled = true;
                }
            }
            else
            {
                if (currentDramaIndex != AllDramas.SelectedIndex)
                {
                    VideoInfo info = AllDramas.SelectedItem as VideoInfo;
                    if (info == null) return;
                    App.PlayerModel.VideoId = info.videoId;
                    App.PlayerModel.ReloadNewVideo();
                    App.PlayerModel.PlayerVideo(info);
                }
                currentDramaIndex = AllDramas.SelectedIndex;
                RelatedVideos.SelectedIndex = -1;
            }
        }
        private void CommentItem_Loaded(object sender, RoutedEventArgs e)
        {
            //App.PlayerModel.LoadedComment();
        }

        private void RelatedItem_Loaded(object sender, RoutedEventArgs e)
        {
            //App.PlayerModel.LoadRrelatedVideo();
        }
        private void DetailItem_Loaded(object sender, RoutedEventArgs e)
        {
            //App.PlayerModel.LoadedDetail();
        }
        private void Related_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RelatedVideos.SelectedIndex == -1) return;
            VideoInfo info = RelatedVideos.SelectedItem as VideoInfo;
            if (info == null) return;
            App.PlayerModel.VideoId = info.videoId;
            AllDramas.SelectedIndex = -1;
            App.PlayerModel.PlayerVideo(info);
        }

        #region 视频播放处理方法
        // 使用定时器来处理视频播放的进度条
        DispatcherTimer currentPosition = new DispatcherTimer(); 
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            //定义多媒体流可用并被打开时触发的事件
            myMediaElement.MediaOpened += new RoutedEventHandler(myMediaElement_MediaOpened);
            //定义多媒体停止时触发的事件
            myMediaElement.MediaEnded += new RoutedEventHandler(myMediaElement_MediaEnded);
            //定义多媒体播放状态改变时触发的事件
            myMediaElement.CurrentStateChanged += new RoutedEventHandler(myMediaElement_CurrentStateChanged);
            //定义定时器触发的事件
            currentPosition.Tick += new EventHandler(currentPosition_Tick);
          
        }


        //视频状态改变时的处理事件
        void myMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (myMediaElement.CurrentState == MediaElementState.Playing)
            {//播放视频时各菜单的状态
                if (WpStorage.GetIsoSetting(needSetSliderValueIso) != null)
                {
                    needSetSliderValue = (bool)WpStorage.GetIsoSetting(needSetSliderValueIso);
                    if (needSetSliderValue)
                    {
                        needSetSliderValue = false;
                        WpStorage.SetIsoSetting(needSetSliderValueIso, needSetSliderValue);
                        if (WpStorage.GetIsoSetting(leaveSilderValueIso) != null) 
                        {
                            leaveSilderValue = (double)WpStorage.GetIsoSetting(leaveSilderValueIso);
                            pbVideo.Tag = "isFoucesed";
                            pbVideo.Value = leaveSilderValue;
                        }
                     
                    }
                 
                }
                currentPosition.Start();
                App.HideLoading();
                App.PlayerModel.LoadVisibility = Visibility.Collapsed;
                PlayImg.Source = new BitmapImage(new Uri(pauseImg, UriKind.RelativeOrAbsolute));
            }
            else if (myMediaElement.CurrentState == MediaElementState.Paused)
            { //暂停视频时各菜单的状态
                currentPosition.Stop();
                PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
            }
            else
            {//停止视频时各菜单的状态
                currentPosition.Stop();
                PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
            }
        }
        //多媒体停止时触发的事件
        void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            myMediaElement.BufferingProgressChanged -= new RoutedEventHandler(MediaBufferChannged);
            //停止播放
            myMediaElement.Stop();
        }
        //多媒体流可用并被打开时触发的事件
        void myMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            pbVideo.IsEnabled = true;
            //获取多媒体视频的总时长来设置进度条的最大值
            pbVideo.Maximum = (int)myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            string endtext = myMediaElement.NaturalDuration.TimeSpan.ToString();
            if (endtext.IndexOf(".") != -1)
            {
                EndTextBlock.Text = endtext.Substring(0, endtext.IndexOf("."));
            }
            else 
            {
                EndTextBlock.Text = endtext;
            }
            myMediaElement.BufferingProgressChanged += new RoutedEventHandler(MediaBufferChannged);
            //播放视频
            myMediaElement.Play();
           
        }

        private void MediaBufferChannged(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("正在加载");
            App.ShowLoading();
            App.PlayerModel.LoadVisibility = Visibility.Visible;
        }
        private void pbVideo_GotFocus(object sender, RoutedEventArgs e)
        {
            pbVideo.Tag = "isFoucesed";
            currentPosition.Stop();
        }

        private void pbVideo_LostFocus(object sender, RoutedEventArgs e)
        {
            pbVideo.Tag = "loseFoucesed";
            currentPosition.Start();
        }
        private void pbVideo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //判断是否用户拖动  false则为计时器触发
            if (pbVideo.Tag != null)
            {
                string silderTag = pbVideo.Tag.ToString();
                if (silderTag.Equals("isFoucesed"))
                {
                    int sliderNewVlaue = (int)e.NewValue;
                    int sliderValue = (int)e.OldValue;
                    TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, sliderValue);
                    TimeSpan newtimeSpan = new TimeSpan(0, 0, 0, 0, sliderValue);
                    myMediaElement.Position = timeSpan;
                    fullScreen.Focus();
                    pbVideo.Tag = "loseFoucesed";
                    //myMediaElement.BufferingTime = timeSpan - newtimeSpan;
                }
            }
        }
        //定时器触发的事件
        void currentPosition_Tick(object sender, EventArgs e)
        {
            //获取当前视频播放了的时长来设置进度条的值
            pbVideo.Value = myMediaElement.Position.TotalMilliseconds;
            string start = myMediaElement.Position.ToString();
            StartTextBlock.Text = start.Substring(3, 5);
        }
        //播放视频菜单事件
        private void Play_Click(object sender, EventArgs e)
        {
            myMediaElement.Play();
        }
        //暂停视频菜单事件
        private void Pause_Click(object sender, EventArgs e)
        {
            myMediaElement.Pause();
        }
        //停止视频菜单事件
        private void Stop_Click(object sender, EventArgs e)
        {
            myMediaElement.Stop();
        }

        private void PlayerGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            playImgStatus = !playImgStatus;
            if (playImgStatus)
            {
                myMediaElement.Play();
                PlayImg.Source = new BitmapImage(new Uri(pauseImg, UriKind.RelativeOrAbsolute));

            }
            else 
            {
                myMediaElement.Pause();
                PlayImg.Source = new BitmapImage(new Uri(playImg, UriKind.RelativeOrAbsolute));
            }
        }

        #endregion

        #region applicationBar method
        private ApplicationBarIconButton startDownBtn;
        private void DownLoad_Click(object sender, EventArgs e)
        {
            MainPivot.SelectedIndex = 0;
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string closeIcon = "/Images/Icons/cancel.png";
            ApplicationBarIconButton closeBtn = new ApplicationBarIconButton(new Uri(closeIcon, UriKind.Relative));
            closeBtn.Text = "取消";
            closeBtn.Click += new EventHandler(CloseIcon_Click);
            this.ApplicationBar.Buttons.Add(closeBtn);

            string startDownIcon = "/Images/Icons/check.png";
            startDownBtn = new ApplicationBarIconButton(new Uri(startDownIcon, UriKind.Relative));
            startDownBtn.Text = "开始缓存";
            startDownBtn.Click += new EventHandler(StartDownIcon_Click);
            this.ApplicationBar.Buttons.Add(startDownBtn);

            AllDramas.ItemContainerStyle = App.PlayerModel.MultipleVideoStyle;
            AllDramas.SelectionMode = SelectionMode.Multiple;
            IsDownMode = true;

        }
        private void CloseIcon_Click(object sender, EventArgs e)
        {
            LoadDownIcon();
            AllDramas.ItemContainerStyle = App.PlayerModel.VideoStyle;
            AllDramas.SelectionMode = SelectionMode.Single;
            IsDownMode = false;
            LoadDramaSeletedItem(App.PlayerModel.VideoId,true);
        }
        private void StartDownIcon_Click(object sender, EventArgs e)
        {
            foreach (VideoInfo videoInfo in AllDramas.SelectedItems) 
            {
                App.DownVideoModel.AddDownVideo(videoInfo);
            }
            LoadDownIcon();
            AllDramas.ItemContainerStyle = App.PlayerModel.VideoStyle;
            AllDramas.SelectionMode = SelectionMode.Single;
            IsDownMode = false;
            LoadDramaSeletedItem(App.PlayerModel.VideoId, true);
        }
       
        private void LoadDownIcon()
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string downIcon = "/Images/Icons/download.png";
            ApplicationBarIconButton downBtn = new ApplicationBarIconButton(new Uri(downIcon, UriKind.Relative));
            downBtn.Text = "下载";
            downBtn.Click += new EventHandler(DownLoad_Click);
            this.ApplicationBar.Buttons.Add(downBtn);

        }
        private void ShowDownVideos_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(CommonData.DownVideoPage, UriKind.RelativeOrAbsolute));
        }
        #endregion

        private void fullScreen_Click(object sender, RoutedEventArgs e)
        {
            //myMediaElement.Height = PopupManager.screenWidth;
            //myMediaElement.Width = PopupManager.screenHeight;
            // RotateTransform rotateTransform = new RotateTransform();
            // rotateTransform.Angle = 90;
            // rotateTransform.CenterX = PopupManager.screenWidth / 2;
            // rotateTransform.CenterY = PopupManager.screenHeight / 2;
            // myMediaElement.RenderTransform = rotateTransform;
        }




     
       
    }
}