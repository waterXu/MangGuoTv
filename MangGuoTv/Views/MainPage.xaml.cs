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
        }
        #region control event func
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
                if (siftListLoadSucc)
                {
                    SiftLLs.ItemsSource = TemplateSiftListData;
                }
                else 
                {
                    SiftLLs_Loaded(null, null);
                }
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 1) 
            {
                ChannelInfo channel = FindChannelInfo("热榜");
                if (rankListLoadSucc)
                {
                    RankListBox.ItemsSource = rankListData;
                }
                else 
                {
                    RankListBox_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                FunLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 2)
            {
                ChannelInfo channel = FindChannelInfo("综艺");
                if (funListLoadSucc)
                {
                    FunLLs.ItemsSource = TemplateFunListData;
                }
                else 
                {
                    FunLLs_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                TvLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 3)
            {
                ChannelInfo channel = FindChannelInfo("电视剧");
                if (tvListLoadSucc)
                {
                    TvLLs.ItemsSource = TemplateTvListData;
                }
                else
                {
                    TvLLs_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 4)
            {
                ChannelInfo channel = FindChannelInfo("电影");
                if (movieListLoadSucc)
                {
                    MovieLLs.ItemsSource = TemplateMovieListData;
                }
                else
                {
                    MovieLLs_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 5)
            {
                ChannelInfo channel = FindChannelInfo("动漫");
                if (animeListLoadSucc)
                {
                    AnimeLLs.ItemsSource = TemplateAnimeListData;
                }
                else
                {
                    AnimeLLs_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
            else if (currentPovitIndex == 6)
            {
                ChannelInfo channel = FindChannelInfo("少儿");
                if (childListLoadSucc)
                {
                    ChildLLs.ItemsSource = TemplateChildListData;
                }
                else
                {
                    ChildLLs_Loaded(null, null);
                }
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
            }
            else
            {
                SiftLLs.ItemsSource = null;
                RankListBox.ItemsSource = null;
                FunLLs.ItemsSource = null;
                MovieLLs.ItemsSource = null;
                AnimeLLs.ItemsSource = null;
                TvLLs.ItemsSource = null;
                ChildLLs.ItemsSource = null;
            }
        }
        bool isNeedLoadChach = false;
        private void CleanItemsSource() 
        {
            SiftLLs.ItemsSource = null;
            RankListBox.ItemsSource = null;
            FunLLs.ItemsSource = null;
            MovieLLs.ItemsSource = null;
            AnimeLLs.ItemsSource = null;
            TvLLs.ItemsSource = null;
            ChildLLs.ItemsSource = null;
            isNeedLoadChach = true;
        }
        private void LoadChachItemsSource() 
        {
            switch (currentPovitIndex) 
            {
                case 0:
                    if (siftListLoadSucc)
                    {
                        SiftLLs.ItemsSource = TemplateSiftListData;
                    }
                    else
                    {
                        SiftLLs_Loaded(null, null);
                    }
                    break;
                case 1:
                    if (rankListLoadSucc)
                    {
                        RankListBox.ItemsSource = rankListData;
                    }
                    else
                    {
                        RankListBox_Loaded(null, null);
                    }
                    break;
                case 2:
                    if (funListLoadSucc)
                    {
                        FunLLs.ItemsSource = TemplateFunListData;
                    }
                    else
                    {
                        FunLLs_Loaded(null, null);
                    }
                    break;
                case 3:
                    if (tvListLoadSucc)
                    {
                        TvLLs.ItemsSource = TemplateTvListData;
                    }
                    else
                    {
                        TvLLs_Loaded(null, null);
                    }
                    break;
                case 4:
                    if (movieListLoadSucc)
                    {
                        MovieLLs.ItemsSource = TemplateMovieListData;
                    }
                    else
                    {
                        MovieLLs_Loaded(null, null);
                    }
                    break;
                case 5:
                    if (animeListLoadSucc)
                    {
                        AnimeLLs.ItemsSource = TemplateAnimeListData;
                    }
                    else
                    {
                        AnimeLLs_Loaded(null, null);
                    }
                    break;
                case 6:
                    if (childListLoadSucc)
                    {
                        ChildLLs.ItemsSource = TemplateChildListData;
                    }
                    else
                    {
                        ChildLLs_Loaded(null, null);
                    }
                    break;
               
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.MainViewModel;
            if (isNeedLoadChach)
            {
                LoadChachItemsSource();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
 	         base.OnNavigatedFrom(e);
             this.DataContext = null;
             CallbackManager.currentPage = null;
             if (e.Content != null)
             {
                 CleanItemsSource();
             }
             App.ShowMemory();

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
        #endregion

        #region lls loadData
        bool rankListLoadSucc = false;
        private void RankListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (rankListLoadSucc) { return; }
            ChannelInfo rankChannel = FindChannelInfo("热榜");
            if (rankChannel != null)
            {
                App.ShowLoading();
                string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + rankChannel.channelId + "&type=" + rankChannel.type;
                HttpHelper.httpGet(channelInfoUrl, LoadRankChannelCompleted);
                System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            }
            else 
            {
                App.ShowToast("对不起，获取该频道信息失败");
            }
            
        }
        List<ChannelTemplate> rankListData = new List<ChannelTemplate>();
        private void LoadRankChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
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
                    RankLoadGrid.Visibility = System.Windows.Visibility.Visible;
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
        public enum ChannelType
        {
            Sift,
            Fun,
            Tv,
            Movie,
            Anime,
            Child
        }
        bool siftListLoadSucc = false;
        private void SiftLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (siftListLoadSucc) { return; }
            LoadChannelData("精选",ChannelType.Sift);
        }
        bool funListLoadSucc = false;
        private void FunLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (funListLoadSucc) { return; }
            LoadChannelData("综艺", ChannelType.Fun);
        }
        bool tvListLoadSucc = false;
        private void TvLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (tvListLoadSucc) { return; }
            LoadChannelData("电视剧", ChannelType.Tv);
        }
        bool movieListLoadSucc = false;
        private void MovieLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (movieListLoadSucc) { return; }
            LoadChannelData("电影", ChannelType.Movie);
        }
        bool animeListLoadSucc = false;
        private void AnimeLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (animeListLoadSucc) { return; }
            LoadChannelData("动漫", ChannelType.Anime);
        }
        bool childListLoadSucc = false;
        private void ChildLLs_Loaded(object sender, RoutedEventArgs e)
        {
            if (childListLoadSucc) { return; }
            LoadChannelData("少儿", ChannelType.Child);
        }

        private void LoadChannelData(string name,ChannelType type) 
        {
            ChannelInfo channel = FindChannelInfo(name);
            if (channel != null)
            {
                App.ShowLoading();
                string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + channel.channelId + "&type=" + channel.type;
                switch (type)
                {
                    case ChannelType.Sift:
                        HttpHelper.httpGet(channelInfoUrl, LoadSiftChannelCompleted);
                        break;
                    case ChannelType.Fun:
                        HttpHelper.httpGet(channelInfoUrl, LoadFunChannelCompleted);
                        break;
                    case ChannelType.Tv:
                        HttpHelper.httpGet(channelInfoUrl, LoadTvChannelCompleted);
                        break;
                    case ChannelType.Movie:
                        HttpHelper.httpGet(channelInfoUrl, LoadMovieChannelCompleted);
                        break;
                    case ChannelType.Anime:
                        HttpHelper.httpGet(channelInfoUrl, LoadAnimeChannelCompleted);
                        break;
                    case ChannelType.Child:
                        HttpHelper.httpGet(channelInfoUrl, LoadChildChannelCompleted);
                        break;
                }
              
                System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            }
            else
            {
                if (name == "少儿") 
                {
                    string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=1021" + "&type=normal";
                }
                else 
                {
                    App.ShowToast("获取该频道信息失败");
                }
            }
        }

        private void LoadSiftChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Sift);
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
                    SiftLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void LoadFunChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Fun);
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    funLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    FunLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void LoadTvChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Tv);
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    tvLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    TvLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void LoadMovieChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Movie);
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    movieLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    MovieLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void LoadAnimeChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Anime);
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    animeLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    AnimeLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        private void LoadChildChannelCompleted(IAsyncResult ar)
        {
            App.HideLoading();
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                convertResultData(result, ChannelType.Child);
            }
            else
            {
                if (CommonData.NetworkStatus != "None")
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    childLoadGrid.Visibility = System.Windows.Visibility.Visible;
                    ChildLLs.Visibility = System.Windows.Visibility.Collapsed;
                });

            }
        }
        List<VideoViewModel> TemplateSiftListData = new List<VideoViewModel>();
        List<VideoViewModel> TemplateFunListData = new List<VideoViewModel>();
        List<VideoViewModel> TemplateTvListData = new List<VideoViewModel>();
        List<VideoViewModel> TemplateMovieListData = new List<VideoViewModel>();
        List<VideoViewModel> TemplateAnimeListData = new List<VideoViewModel>();
        List<VideoViewModel> TemplateChildListData = new List<VideoViewModel>();

        private void convertResultData(string result,ChannelType type)
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
                            case "moviePortrait":
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
                                case "live":
                                    break;
                                case "normalAvatorText":
                                case "normalLandScape":
                                case "roundAvatorText":
                                case "tvPortrait":
                                case "moviePortrait":
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
                    switch (type) 
                    {
                        case ChannelType.Sift:
                            if (currentPovitIndex == 0) {
                                SiftLLs.ItemsSource = TemplateListData;
                            }
                            TemplateSiftListData = TemplateListData;
                            siftListLoadSucc = true;
                            break;
                        case ChannelType.Fun:
                            if (currentPovitIndex == 2)
                            {
                                FunLLs.ItemsSource = TemplateListData;
                            }
                            TemplateFunListData = TemplateListData;
                            funListLoadSucc = true;
                            break;
                        case ChannelType.Tv:
                            if (currentPovitIndex == 3)
                            {
                                TvLLs.ItemsSource = TemplateListData;
                            }
                            TemplateTvListData = TemplateListData;
                            tvListLoadSucc = true;
                            break;
                        case ChannelType.Movie:
                            if (currentPovitIndex == 4)
                            {
                                MovieLLs.ItemsSource = TemplateListData;
                            }
                            TemplateMovieListData = TemplateListData;
                            movieListLoadSucc = true;
                            break;
                        case ChannelType.Anime:
                            if (currentPovitIndex == 5)
                            {
                                AnimeLLs.ItemsSource = TemplateListData;
                            }
                            TemplateAnimeListData = TemplateListData;
                            animeListLoadSucc = true;
                            break;
                        case ChannelType.Child:
                            if (currentPovitIndex == 6)
                            {
                                ChildLLs.ItemsSource = TemplateListData;
                            }
                            TemplateChildListData = TemplateListData;
                            childListLoadSucc = true;
                            break;
                    }
                });
            }
        }
        private void ReloadSiftDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            siftLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            SiftLLs_Loaded(null, null);
        }
        private void ReloadFunDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            funLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            FunLLs_Loaded(null, null);
        }
        private void ReloadTvDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            tvLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            TvLLs_Loaded(null, null);
        }
        private void ReloadMovieDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            movieLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            MovieLLs_Loaded(null, null);
        }
        private void ReloadAnimeDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            animeLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            AnimeLLs_Loaded(null, null);
        }
        private void ReloadChildDataTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            childLoadGrid.Visibility = System.Windows.Visibility.Collapsed;
            ChildLLs_Loaded(null, null);
        }
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
        private void FunLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = FunLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }
        private void TvLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = TvLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }
        private void MovieLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = MovieLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }
        private void AnimeLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = AnimeLLs.SelectedItem as VideoViewModel;
            if (template != null)
            {
                OperationImageTap(template);
            }
        }
        private void ChildLLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tapFromGrid)
            {
                tapFromGrid = false;
                return;
            }
            VideoViewModel template = ChildLLs.SelectedItem as VideoViewModel;
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
        #endregion
    }
}