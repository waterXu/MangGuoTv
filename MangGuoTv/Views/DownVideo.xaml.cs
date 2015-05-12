using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.ViewModels;
using MangGuoTv.PopUp;
using Microsoft.Phone.Tasks;

namespace MangGuoTv.Views
{
    public partial class DownVideo : PhoneApplicationPage
    {
        public DownVideo()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.DownVideoModel;
            App.DownVideoModel.BeginDownVideos();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            this.DataContext = null;
        }
        private void DownedVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DownVideoInfoViewMoel DowmVideo = DownVideoList.SelectedItem as DownVideoInfoViewMoel;
            //创建一个多媒体的启动器
            MediaPlayerLauncher mpl = new MediaPlayerLauncher();
            //设置播放文件放置的位置属性 
            mpl.Location = MediaLocationType.Data;
            //设置所有控制纽都出现 
            mpl.Controls = MediaPlaybackControls.All;
            //设置出现停止按钮以及暂停按钮 
            mpl.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
            //设置播放的文件 
            mpl.Media = new Uri(DowmVideo.LocalDownloadUrl, UriKind.Relative);
            //启动播放
            mpl.Show(); 
        }
    }
}