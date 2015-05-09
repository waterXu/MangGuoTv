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

namespace MangGuoTv
{
    public partial class PlayerInfo : PhoneApplicationPage
    {
        private bool IsDownMode = false;
        public PlayerInfo()
        {
            InitializeComponent();
            this.DataContext = App.PlayerModel;
            LoadDownIcon();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            currentPosition.Stop();
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
            }
        }
        private int currentDramaIndex = -1;
        private void AllDramas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllDramas.SelectedIndex == -1) return;
            if (IsDownMode) 
            {
                if (AllDramas.SelectedItems.Count == 0) { } 
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
                    PlayerVideo(info);
                }
                currentDramaIndex = AllDramas.SelectedIndex;
                RelatedVideos.SelectedIndex = -1;
            }
        }
        public void PlayerVideo(VideoInfo info) 
        {
            //设置多媒体控件的网络视频资源
            if(info.downloadUrl.Count > 0)
            {
                string playUrl = info.downloadUrl[0].url;
                System.Diagnostics.Debug.WriteLine("获取播放源：" + playUrl);
                HttpHelper.httpGet(playUrl, (ar) =>
                {
                    string result = HttpHelper.SyncResultTostring(ar);
                    if (result != null)
                    {
                        ResourceInfo videosResult = null;
                        try
                        {
                            videosResult = JsonConvert.DeserializeObject<ResourceInfo>(result);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        }
                        if (videosResult != null && videosResult.status == "ok" && videosResult.info != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                myMediaElement.Source = new Uri(videosResult.info, UriKind.RelativeOrAbsolute);
                                System.Diagnostics.Debug.WriteLine("视频地址 ： " + videosResult.info);
                            });
                        }
                    }
                    else
                    {
                        App.ShowToast("获取视频数据失败，请检查网络或重试");
                    }
                });
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
            PlayerVideo(info);
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
                currentPosition.Start();
            }
            else if (myMediaElement.CurrentState == MediaElementState.Paused)
            { //暂停视频时各菜单的状态
                currentPosition.Stop();
            }
            else
            {//停止视频时各菜单的状态
                currentPosition.Stop();
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
            EndTextBlock.Text = endtext.Substring(3, 5);
            myMediaElement.BufferingProgressChanged += new RoutedEventHandler(MediaBufferChannged);
            //播放视频
            myMediaElement.Play();
        }

        private void MediaBufferChannged(object sender, RoutedEventArgs e)
        {
            
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
                    myMediaElement.BufferingTime = timeSpan - newtimeSpan;
                }
            }
        }
        //定时器触发的事件
        void currentPosition_Tick(object sender, EventArgs e)
        {
            //获取当前视频播放了的时长来设置进度条的值
            pbVideo.Value = (int)myMediaElement.Position.TotalMilliseconds;
            string start = myMediaElement.Position.ToString();
            StartTextBlock.Text = start.Substring(3, 5);
            pbVideo.Tag = "loseFoucesed";

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
        #endregion

        #region applicationBar method
        private ApplicationBarIconButton startDownBtn;
        private void DownLoad_Click(object sender, EventArgs e)
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string closeIcon = "/Images/Icons/close.png";
            ApplicationBarIconButton closeBtn = new ApplicationBarIconButton(new Uri(closeIcon, UriKind.Relative));
            closeBtn.Text = "取消";
            closeBtn.Click += new EventHandler(CloseIcon_Click);
            this.ApplicationBar.Buttons.Add(closeBtn);

            string startDownIcon = "/Images/Icons/close.png";
            startDownBtn = new ApplicationBarIconButton(new Uri(startDownIcon, UriKind.Relative));
            startDownBtn.Text = "开始缓存";
            startDownBtn.Click += new EventHandler(StartDownIcon_Click);
            startDownBtn.IsEnabled = false;
            this.ApplicationBar.Buttons.Add(startDownBtn);

           
            Style itemStyle = Resources["ListBoxItemStyle1"] as Style;
            AllDramas.ItemContainerStyle = itemStyle;
            AllDramas.SelectionMode = SelectionMode.Multiple;
            IsDownMode = true;

        }
        private void CloseIcon_Click(object sender, EventArgs e)
        {
            LoadDownIcon();
            Style itemStyle = Resources["ListBoxItemStyle"] as Style;
            AllDramas.ItemContainerStyle = itemStyle;
            AllDramas.SelectionMode = SelectionMode.Single;
            IsDownMode = false;
            LoadDramaSeletedItem(App.PlayerModel.VideoId,true);
        }
        private void StartDownIcon_Click(object sender, EventArgs e)
        {

        }
       
        private void LoadDownIcon()
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string downIcon = "/Images/Icons/down.png";
            ApplicationBarIconButton downBtn = new ApplicationBarIconButton(new Uri(downIcon, UriKind.Relative));
            downBtn.Text = "下载";
            downBtn.Click += new EventHandler(DownLoad_Click);
            this.ApplicationBar.Buttons.Add(downBtn);

        }
        private void ShowDownVideos_Click(object sender, EventArgs e)
        {

        }
        #endregion




     
       
    }
}