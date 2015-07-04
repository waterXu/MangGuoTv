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
using MangGuoTv.Popups;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;

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
           
            //for (int i = 0; i < CommonData.LockedChannel.Count; i++)
            //{
            //    if (CommonData.LockedChannel[i].channelName == "精选" || CommonData.LockedChannel[i].channelName == "热榜")
            //    {
            //        PivotItemControl pivot = new PivotItemControl(CommonData.LockedChannel[i]);
            //        pivot.pivotItem.DataContext = CommonData.LockedChannel[i];
            //        MainPivot.Items.Add(pivot.pivotItem);
            //    }
           
            //}
            //for (int i = 0; i < CommonData.NormalChannel.Count; i++)
            //{
            //    if (CommonData.NormalChannel[i].channelName == "精选" || CommonData.NormalChannel[i].channelName == "热榜" )
            //    {
            //        PivotItemControl pivot = new PivotItemControl(CommonData.NormalChannel[i]);
            //        pivot.pivotItem.DataContext = CommonData.NormalChannel[i];
            //        MainPivot.Items.Add(pivot.pivotItem);
            //    }
            //}
            //MainPivot.SelectedIndex = 2;
        }
       
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            CallbackManager.Mainpage = this;
        }
        int currentPovitIndex = 0;
        public ChannelInfo channel { get; set; }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPovitIndex = MainPivot.SelectedIndex;
            if (currentPovitIndex == 0) 
            {
                ChannelInfo channel = FindChannelInfo("精选");
            }
            else if (currentPovitIndex == 1) 
            {
                ChannelInfo channel = FindChannelInfo("热榜");
            }
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
            if (PopupManager.HasPopup()) 
            {
                PopupManager.OffPopUp();
                e.Cancel = true;
                return;
            }
            if (App.DownVideoModel.DowningVideoids != null && App.DownVideoModel.DowningVideoids.Count > 0)
            {
                if (MessageBox.Show("还有正在下载的剧集，确定要退出吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    App.DownVideoModel.SaveVideoData();
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


        private ChannelInfo FindChannelInfo(string channelName)
        {
            foreach (ChannelInfo channel in App.MainViewModel.AllChannels) {
                if (channel.channelName == channelName)
                {
                    return channel;
                }
            }
            return null;
        }
        #region loadData
        bool rankListLoadSucc = false;
        private void RankListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (rankListLoadSucc) { return; }
            ChannelInfo rankChannel = FindChannelInfo("热榜");
            if (rankChannel != null)
            {
                string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + rankChannel.channelId + "&type=" + rankChannel.type;
                HttpHelper.httpGet(channelInfoUrl, LoadRankChannelCompleted);
                System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            }
            else 
            {
                MessageBox.Show("获取该频道信息失败");
            }
            
        }
        private void LoadRankChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                channelDetailResult channelDetails = null;
                try
                {
                    channelDetails = JsonConvert.DeserializeObject<channelDetailResult>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                }
                if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
                {
                    rankListLoadSucc = true;

                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                    {
                        RankListBox.Visibility = System.Windows.Visibility.Visible;
                       List<ChannelTemplate> rankListData = new List<ChannelTemplate>();
                       foreach (ChannelDetail channelDatail in channelDetails.data)
                       {
                           rankListData.AddRange(channelDatail.templateData);
                       }
                       RankListBox.ItemsSource = rankListData;
                    });
                }
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    App.HideLoading();
                    RankLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    //RankLoadGrid.Tap -= new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadRankDataTap);
                    //RankLoadGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadRankDataTap);
                    RankListBox.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }

        private void ReloadRankDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RankLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            RankListBox_Loaded(null, null);
        }

        private void RankListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChannelTemplate template = RankListBox.SelectedItem as ChannelTemplate;
            if (template != null) 
            {
                VideoViewModel videoData = new VideoViewModel
                {
                    width = 150,
                    hight = 130,
                    name = template.name,
                    jumpType = template.jumpType,
                    subjectId = template.subjectId,
                    picUrl = template.picUrl,
                    playUrl = template.playUrl,
                    tag = template.tag,
                    desc = template.desc,
                    videoId = template.videoId,
                    hotDegree = template.hotDegree,
                    webUrl = template.webUrl,
                    rank = template.rank
                };
                OperationImageTap(videoData);
            }
        }

        bool siftListLoadSucc = false;
        private void SiftLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (siftListLoadSucc) { return; }
            ChannelInfo rankChannel = FindChannelInfo("精选");
            if (rankChannel != null)
            {
                string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + rankChannel.channelId + "&type=" + rankChannel.type;
                HttpHelper.httpGet(channelInfoUrl, LoadSiftChannelCompleted);
                System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            }
            else
            {
                MessageBox.Show("获取该频道信息失败");
            }

        }
        private void LoadSiftChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                channelDetailResult channelDetails = null;
                try
                {
                    channelDetails = JsonConvert.DeserializeObject<channelDetailResult>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                }
                if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
                {
                    siftListLoadSucc = true;

                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                    {
                        SiftLLs.Visibility = System.Windows.Visibility.Visible;
                        List<VideoViewModel> TemplateListData = new List<VideoViewModel>();
                        foreach (ChannelDetail channelDatail in channelDetails.data)
                        {
                            switch (channelDatail.type)
                            {
                                case "normalAvatorText":
                                case "normalLandScape":
                                case "roundAvatorText":
                                case "tvPortrait":
                                    for (int i = 0; i < channelDatail.templateData.Count; i=i+2)
                                    {
                                        if (channelDatail.templateData.Count > i + 1)
                                        {
                                            ChannelTemplate template1 = channelDatail.templateData[i];
                                            ChannelTemplate template2 = channelDatail.templateData[i+1];
                                            VideoViewModel videoData = new VideoViewModel
                                            {
                                                //stupid func
                                                type = channelDatail.type,
                                                name = template1.name,
                                                jumpType = template1.jumpType,
                                                picUrl = template1.picUrl,
                                                tag = template1.tag,
                                                desc = template1.desc,
                                                videoId = template1.videoId,
                                                webUrl = template1.webUrl,
                                                playUrl = template1.playUrl,
                                                subjectId = template1.subjectId,
                                                name1 = template2.name,
                                                picUrl1 = template2.picUrl,
                                                tag1 = template2.tag,
                                                desc1 = template2.desc,
                                                videoId1 = template2.videoId,
                                                jumpType1 = template1.jumpType,
                                                webUrl1 = template2.webUrl,
                                                playUrl1 = template2.playUrl,
                                                subjectId1 = template2.subjectId,
                                            };
                                            TemplateListData.Add(videoData);
                                        }
                                        else 
                                        {
                                            ChannelTemplate template1 = channelDatail.templateData[i];
                                            VideoViewModel videoData = new VideoViewModel
                                            {
                                                type = channelDatail.type,
                                                name = template1.name,
                                                jumpType = template1.jumpType,
                                                picUrl = template1.picUrl,
                                                tag = template1.tag,
                                                desc = template1.desc,
                                                videoId = template1.videoId,
                                                webUrl = template1.webUrl,
                                                playUrl = template1.playUrl,
                                                subjectId = template1.subjectId,
                                            };
                                            TemplateListData.Add(videoData);
                                        }
                                      
                                    }
                                    break;
                                default:
                                    break;
                            }
                            foreach (ChannelTemplate template in channelDatail.templateData)
                            {
                                switch (channelDatail.type)
                                {
                                    case "banner":
                                    case "largeLandScapeNodesc":
                                    case "largeLandScape":
                                    case "normalLandScapeNodesc":
                                    case "aceSeason":
                                    case "title":
                                    case "rankList":
                                    case "live":
                                        break;
                                    case "normalAvatorText":
                                    case "normalLandScape":
                                    case "roundAvatorText":
                                    case "tvPortrait":
                                    case "unknowModType1":
                                    case "unknowModType2":
                                        continue;
                                    default:
                                        continue;
                                }
                                VideoViewModel videoData = new VideoViewModel
                                {
                                    type = channelDatail.type,
                                    //width = width,
                                    //hight = height,
                                    name = template.name,
                                    jumpType = template.jumpType,
                                    subjectId = template.subjectId,
                                    picUrl = template.picUrl,
                                    playUrl = template.playUrl,
                                    tag = template.tag,
                                    desc = template.desc,
                                    videoId = template.videoId,
                                    hotDegree = template.hotDegree,
                                    webUrl = template.webUrl,
                                    rank = template.rank
                                };
                                TemplateListData.Add(videoData);
                            }
                        }
                        SiftLLs.ItemsSource = TemplateListData;
                    });
                }
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    siftLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    App.HideLoading();
                    //siftLoadGrid.Tap -= new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadSiftDataTap);
                    //siftLoadGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadSiftDataTap);
                    SiftLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void ReloadSiftDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            siftLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            SiftLLs_Loaded(null, null);
        }

        #endregion 

        private void SiftLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid) 
            {
                tapFromGrid = false;
                return;
            }

            VideoViewModel template = SiftLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }
        private void OperationImageTap(VideoViewModel template)
        {
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif

            switch (template.jumpType)
            {
                case "videoPlayer":
                    if (template.type != "title") 
                    {
                        App.PlayerModel.VideoId = template.videoId;
                        App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
                        CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative));
                    }
                    break;

                case "subjectPage":
                    MoreSubject.subjectId = template.subjectId;
                    MoreSubject.speicalName = template.name;
                    MoreSubject.isMoreChannel = false;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
                    break;
                case "videoLibrary":
                    if (channel != null)
                    {
                        MoreChannelInfo.typeId = channel.libId;
                        MoreChannelInfo.name = channel.channelName;
                        CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.MoreChannelPageName, UriKind.Relative));
                    }
                    break;
                case "webView":
                    WebBrowserTask task = new WebBrowserTask();
                    task.Uri = new Uri(template.webUrl, UriKind.Absolute);
                    try
                    {
                        task.Show();
                    }
                    catch (Exception e)
                    {

                    }
                    break;
                case "livePlayer":
                    LivePlayer.liveUrl = template.playUrl;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.LivePlayerPage, UriKind.Relative));
                    break;
                case "concertLivePlayer":
                    LivePlayer.liveUrl = "http://live.api.hunantv.com/mobile/getLiveStreaming?videoId=" + template.videoId;
                    HttpHelper.httpGet(LivePlayer.liveUrl, (ar) =>
                    {
                        string result = HttpHelper.SyncResultTostring(ar);
                        if (result != null)
                        {
                            LiveInfo videosResult = null;
                            try
                            {
                                videosResult = JsonConvert.DeserializeObject<LiveInfo>(result);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                                App.JsonError(result);
                            }
                            if (videosResult != null && videosResult.m3u8 != null)
                            {
                                LivePlayer.liveUrl = videosResult.m3u8;
                                if (!string.IsNullOrEmpty(LivePlayer.liveUrl))
                                {
                                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                                    {
                                        CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.LivePlayerPage, UriKind.Relative));
                                    });

                                }

                            }
                        }
                    });

                    //App.ShowToast("抱歉，暂时不支持直播功能");
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("该播放类型暂时未实现" + template.jumpType);
                    App.ShowToast("该播放类型暂时未实现" + template.jumpType);
                    break;
            }
        }
        bool tapFromGrid = false;
        private void NorVideoTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            tapFromGrid = true;

            Grid tapGrid = sender as Grid;
            if (tapGrid != null) {
                string tag = tapGrid.Tag.ToString();
                VideoViewModel template1 = tapGrid.DataContext as VideoViewModel;
                if (template1 == null) return;
                switch (tag) 
                {
                    case "0":
                         VideoViewModel template = new VideoViewModel
                        {
                            type = template1.type,
                            name = template1.name,
                            jumpType = template1.jumpType,
                            picUrl = template1.picUrl,
                            tag = template1.tag,
                            desc = template1.desc,
                            videoId = template1.videoId,
                            webUrl = template1.webUrl,
                            playUrl = template1.playUrl,
                            subjectId = template1.subjectId,
                        };
               
                        OperationImageTap(template);
                        break;
                    case "1":
                         VideoViewModel template2 = new VideoViewModel
                        {
                            type = template1.type,
                            name = template1.name1,
                            jumpType = template1.jumpType1,
                            picUrl = template1.picUrl1,
                            tag = template1.tag1,
                            desc = template1.desc1,
                            videoId = template1.videoId1,
                            webUrl = template1.webUrl1,
                            playUrl = template1.playUrl1,
                            subjectId = template1.subjectId1,
                        };
                        OperationImageTap(template2);
                        break;
                    default:
                         VideoViewModel template3 = new VideoViewModel
                        {
                            type = template1.type,
                            name = template1.name,
                            jumpType = template1.jumpType,
                            picUrl = template1.picUrl,
                            tag = template1.tag,
                            desc = template1.desc,
                            videoId = template1.videoId,
                            webUrl = template1.webUrl,
                            playUrl = template1.playUrl,
                            subjectId = template1.subjectId,
                        };
                        OperationImageTap(template3);
                        break;
                }
            }
        }
    }
}