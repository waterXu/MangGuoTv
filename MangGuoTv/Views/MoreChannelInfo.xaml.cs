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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Markup;
using MangGuoTv.ViewModels;
using Microsoft.Phone.Info;
using MangGuoTv.Popups;

namespace MangGuoTv.Views
{
    public partial class MoreChannelInfo : PhoneApplicationPage
    {
        private ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        public static string typeId { get; set; }
        public static string name { get; set; }
        public Grid addTipGrid = null;
        private int pageCount = 1;
        public MoreChannelInfo()
        {
            InitializeComponent();
            //scrollView = new ScrollViewer();
            ////scrollView.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //stackPanel = new StackPanel();
            //CreateReload();
            //stackPanel.Children.Add(loadGrid);
            //scrollView.Content = stackPanel;
            //MainGrid.Children.Add(scrollView);
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
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //pageCount = 1;
            //this.channelName.Text = name;
            //string channelInfoUrl = CommonData.GetMoreChannelInfo + "&typeId=" + typeId + "&pageCount=" + pageCount;
            //App.ShowLoading();
            //System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            //HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
        }
        //private void LoadChannelCompleted(IAsyncResult ar)
        //{
        //    string result = HttpHelper.SyncResultTostring(ar);
        //    if (result != null)
        //    {
        //        MoreChannelResult channelDetails = null;
        //        try
        //        {
        //            channelDetails = JsonConvert.DeserializeObject<MoreChannelResult>(result);
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
        //            App.HideLoading();
        //        }
        //        if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
        //        {
        //            this.Dispatcher.BeginInvoke(() =>
        //            {
        //                App.HideLoading();
        //                loadGrid.Visibility = Visibility.Collapsed;
        //                //if (pageCount > 0) 
        //                //{
        //                //    stackPanel.Children.Remove(addTipGrid);
        //                //}
        //                AddChannelView(channelDetails.data, 200, 2);
        //                pageCount++;
        //            });
        //        }
        //    }
        //    else
        //    {
        //        //App.ShowToast("获取数据失败，请检查网络或重试");
        //        App.HideLoading();
        //        this.Dispatcher.BeginInvoke(() =>
        //        {
        //            loadGrid.Visibility = Visibility.Visible;
        //        });
        //    }
        //}


        #region  View event Menthod

        private void MoreChannelData_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
            ReloadDataTap(null, null);
        }
        private void ReloadDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ShowLoading();
            reLoadGrid.Visibility = Visibility.Collapsed;
            string channelInfoUrl = CommonData.GetMoreChannelInfo + "&typeId=" + typeId + "&pageCount=" + pageCount;
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);

        }
        #endregion

        private void SiftLLs_Loaded(object sender, RoutedEventArgs e)
        {
            pageCount = 1;
            this.channelName.Text = name;
            string channelInfoUrl = CommonData.GetMoreChannelInfo + "&typeId=" + typeId + "&pageCount=" + pageCount;
            App.ShowLoading();
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
        }
        private void LoadChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                MoreChannelResult channelDetails = null;
                try
                {
                    channelDetails = JsonConvert.DeserializeObject<MoreChannelResult>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                    App.HideLoading();
                }
                if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
                {
                    this.Dispatcher.BeginInvoke(() =>
                    {
                        App.HideLoading();
                        //loadGrid.Visibility = Visibility.Collapsed;
                        ////if (pageCount > 0) 
                        ////{
                        ////    stackPanel.Children.Remove(addTipGrid);
                        ////}
                        //AddChannelView(channelDetails.data, 200, 2);
                        List<VideoViewModel> TemplateListData = new List<VideoViewModel>();

                        for (int i = 0; i < channelDetails.data.Count; i = i + 2)
                        {
                            if (channelDetails.data.Count > i + 1)
                            {
                                MoreChannel template1 = channelDetails.data[i];
                                MoreChannel template2 = channelDetails.data[i + 1];
                                VideoViewModel videoData = new VideoViewModel
                                {
                                    //stupid func
                                    name = template1.name,
                                    tag = template1.tag,
                                    desc = template1.desc,
                                    picUrl = template1.image,
                                    videoId = template1.videoId,
                                    name1 = template2.name,
                                    tag1 = template2.tag,
                                    desc1 = template2.desc,
                                    videoId1 = template2.videoId,
                                    picUrl1 = template2.image,
                                };
                                TemplateListData.Add(videoData);
                            }
                            else
                            {
                                MoreChannel template1 = channelDetails.data[i];
                                VideoViewModel videoData = new VideoViewModel
                                {
                                    name = template1.name,
                                    tag = template1.tag,
                                    desc = template1.desc,
                                    videoId = template1.videoId,
                                };
                                TemplateListData.Add(videoData);
                            }
                        }
                        moreChannelInfo.ItemsSource = TemplateListData;
                        pageCount++;
                    });
                }
            }
            else
            {
                //App.ShowToast("获取数据失败，请检查网络或重试");
                App.HideLoading();
                this.Dispatcher.BeginInvoke(() =>
                {
                    reLoadGrid.Visibility = Visibility.Visible;
                });
            }
        }

        private void SiftLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void NorVideoTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            Grid tapGrid = sender as Grid;
            if (tapGrid != null)
            {
                string tag = tapGrid.Tag.ToString();
                VideoViewModel template = tapGrid.DataContext as VideoViewModel;
                if (template == null) return;
                switch (tag)
                {
                    case "0":
                        if (string.IsNullOrEmpty(template.videoId)) return;
                        App.PlayerModel.VideoId = template.videoId;
                        App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
                        this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative));
                        break;
                    case "1":
                        if (string.IsNullOrEmpty(template.videoId1)) return;
                        App.PlayerModel.VideoId = template.videoId1;
                        App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
                        this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative));
                        break;
                    default:
                       
                        break;
                }
            }
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
        }

        private void changeDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

    }
}