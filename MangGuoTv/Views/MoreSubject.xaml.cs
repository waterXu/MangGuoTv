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
using MangGuoTv.ViewModels;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Info;

namespace MangGuoTv.Views
{
    public partial class MoreSubject : PhoneApplicationPage
    {
        public static string subjectId { get; set; }
        public static string speicalName { get; set; }
        public static bool isMoreChannel { get; set; }
        public static string channelType { get; set; }


        public MoreSubject()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.ShowMemory();

            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (e.Content != null)
            {
                DataLLs.ItemsSource = null;
            }
        }
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.subjectName.Text = speicalName;
            string channelInfoUrl = null;
            if (isMoreChannel)
            {
                channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + subjectId + "&type=" + channelType;
            }
            else
            {
                channelInfoUrl = CommonData.GetSpecialUrl + "&subjectId=" + subjectId;
            }
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
        }
        private void LoadChannelCompleted(IAsyncResult ar)
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
                    this.Dispatcher.BeginInvoke(() =>
                    {
                        DataLLs.Visibility = System.Windows.Visibility.Visible;
                        List<VideoViewModel> TemplateListData = new List<VideoViewModel>();
                        foreach (ChannelDetail channelDatail in channelDetails.data)
                        {
                            switch (channelDatail.type)
                            {
                                case "normalAvatorText":
                                case "normalLandScape":
                                case "roundAvatorText":
                                case "live":
                                case "tvPortrait":
                                    for (int i = 0; i < channelDatail.templateData.Count; i = i + 2)
                                    {
                                        if (channelDatail.templateData.Count > i + 1)
                                        {
                                            ChannelTemplate template1 = channelDatail.templateData[i];
                                            ChannelTemplate template2 = channelDatail.templateData[i + 1];
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
                                        break;
                                    case "normalAvatorText":
                                    case "live":
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
                        DataLLs.ItemsSource = TemplateListData;
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
                    LoadGrid.Visibility = System.Windows.Visibility.Visible;
                    App.HideLoading();
                    DataLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }

        private void ReloadDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainGrid_Loaded(null, null);
        }

        private void LLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = DataLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }

        private void OperationImageTap(VideoViewModel template)
        {
            App.ShowMemory();
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
                    //if (channel != null)
                    //{
                    //    MoreChannelInfo.typeId = channel.libId;
                    //    MoreChannelInfo.name = channel.channelName;
                    //    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.MoreChannelPageName, UriKind.Relative));
                    //}
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
            if (tapGrid != null)
            {
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