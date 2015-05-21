using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.Resources;
using System.Reflection;
using MangGuoTv.Models;
using Newtonsoft.Json;
using System.IO;
using MangGuoTv.Views;
using MangGuoTv.ViewModels;

namespace MangGuoTv
{
    public partial class MainPage : PhoneApplicationPage
    {
      
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = App.MainViewModel;
            App.MainViewModel.LoadChannels();
           
            for (int i = 0; i < CommonData.LockedChannel.Count; i++)
            {
                if (CommonData.LockedChannel[i].channelName == "精选" || CommonData.LockedChannel[i].channelName == "热榜")
                {
                    PivotItemControl pivot = new PivotItemControl(CommonData.LockedChannel[i]);
                    pivot.pivotItem.DataContext = CommonData.LockedChannel[i];
                    MainPivot.Items.Add(pivot.pivotItem);
                }
           
            }
            for (int i = 0; i < CommonData.NormalChannel.Count; i++)
            {
                if (CommonData.NormalChannel[i].channelName == "精选" || CommonData.NormalChannel[i].channelName == "热榜" )
                {
                    PivotItemControl pivot = new PivotItemControl(CommonData.NormalChannel[i]);
                    pivot.pivotItem.DataContext = CommonData.NormalChannel[i];
                    MainPivot.Items.Add(pivot.pivotItem);
                }
            }
            MainPivot.SelectedIndex = 2;
        }
       
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            CallbackManager.Mainpage = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.MainViewModel;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
 	         base.OnNavigatedFrom(e);
             this.DataContext = null;
             CallbackManager.currentPage = null;
        }

        private void AllChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChannelInfo info = AllChannels.SelectedItem as ChannelInfo;
            if (info != null) 
            {
                if (info.libId == "0")
                {
                    for (int i = 0; i < MainPivot.Items.Count; i++) 
                    {
                        PivotItem pivotItem = MainPivot.Items[i] as PivotItem;
                        ChannelInfo channelinfo = pivotItem.DataContext as ChannelInfo;
                        if (channelinfo != null && channelinfo.channelId == info.channelId) 
                        {
                            MainPivot.SelectedIndex = i;
                            return;
                        }
                        MoreSubject.subjectId = info.channelId;
                        MoreSubject.speicalName = info.channelName;
                        MoreSubject.isMoreChannel = true;
                        MoreSubject.channelType = info.type;
                        CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
                    }
                }
                else 
                {
                    MoreChannelInfo.typeId = info.libId;
                    MoreChannelInfo.name = info.channelName;
                    this.NavigationService.Navigate(new Uri(CommonData.MoreChannelPageName, UriKind.Relative));
                }
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(CommonData.SearchPage,UriKind.RelativeOrAbsolute));
        }

        private void VideoRemember_Changed(object sender, SelectionChangedEventArgs e)
        {
            DownVideoInfoViewMoel DownVideo = VideoRemember.SelectedItem as DownVideoInfoViewMoel;
            if (DownVideo == null) return;
            App.PlayerModel.VideoId = DownVideo.VideoId;
            App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
            this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
        }

        private void AllVideosRemember(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(CommonData.RememberPage, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.DownVideoModel.DowningVideoids != null && App.DownVideoModel.DowningVideoids.Count > 0)
            {
                if (MessageBox.Show("还有正在下载的剧集，确定要推出吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Application.Current.Terminate();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                Application.Current.Terminate();
            }
          
        }

    }
}