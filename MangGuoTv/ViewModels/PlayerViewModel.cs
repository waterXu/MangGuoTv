﻿using MangGuoTv.Models;
using MangGuoTv.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MangGuoTv.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        //播放源类型
        public enum PlayType
        {
            //剧集
            VideoType,
            //花絮
            RelateType,
            //本地
            LoaclType
        }
        public PlayType currentType;
        private string _videoName;
        public string VideoName
        {
            get { return _videoName; }
            set
            {
                if (_videoName != value)
                {
                    _videoName = value;
                    NotifyPropertyChanged("VideoName");
                }
            }
        }
        private ObservableCollection<VideoInfo> _AllDramas;
        public ObservableCollection<VideoInfo> AllDramas
        {
            get
            {
                if (_AllDramas == null)
                {
                    _AllDramas = new ObservableCollection<VideoInfo>();
                }
                return _AllDramas;
            }
            set
            {
                _AllDramas = value;
                NotifyPropertyChanged("AllDramas");
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> downedVideo = App.DownVideoModel.DownedVideo;
        public ObservableCollection<DownVideoInfoViewMoel> DownedVideo
        {
            get
            {
                if (downedVideo == null)
                {
                    downedVideo = new ObservableCollection<DownVideoInfoViewMoel>();
                }
                return downedVideo;
            }
            set
            {
                downedVideo = value;
                NotifyPropertyChanged("DownedVideo");
            }
        }
        private ObservableCollection<VideoInfo> _AllRelateds;
        public ObservableCollection<VideoInfo> AllRelateds
        {
            get
            {
                if (_AllRelateds == null)
                {
                    _AllRelateds = new ObservableCollection<VideoInfo>();
                }
                return _AllRelateds;
            }
            set
            {
                _AllRelateds = value;
                NotifyPropertyChanged("AllRelateds");
            }
        }
        private List<Comment> _comments;
        public List<Comment> Comments
        {
            get
            {
                if (_comments == null)
                {
                    _comments = new List<Comment>();
                }
                return _comments;
            }
            set
            {
                _comments = value;
                NotifyPropertyChanged("Comments");
            }
        }
        private VideoDetail _videoDetail;
        public VideoDetail VideoDetail
        {
            get
            {
                return _videoDetail;
            }
            set
            {
                _videoDetail = value;
                NotifyPropertyChanged("VideoDetail");
            }
        }
        private List<VideoDefinition> _videoSources;
        public List<VideoDefinition> VideoSources
        {
            get
            {
                if (_videoSources == null)
                {
                    _videoSources = new List<VideoDefinition>();
                }
                return _videoSources;
            }
            set
            {
                _videoSources = value;
            }
        }
        private List<VideoDefinition> _downloadUrl;
        public List<VideoDefinition> VideoDownloadUrl
        {
            get
            {
                if (_downloadUrl == null)
                {
                    _downloadUrl = new List<VideoDefinition>();
                }
                return _downloadUrl;
            }
            set
            {
                if (_downloadUrl != value)
                {
                    _downloadUrl = value;
                    NotifyPropertyChanged("VideoDownloadUrl");
                }
            }
        }
        private Uri _mediaSource;
        public Uri MediaSource
        {
            get { return _mediaSource; }
            set
            {
                _mediaSource = value;
                NotifyPropertyChanged("MediaSource");
            }
        }
        private Visibility loadVisibility = Visibility.Visible;
        public Visibility LoadVisibility
        {
            get { return loadVisibility; }
            set
            {
                loadVisibility = value;
                NotifyPropertyChanged("LoadVisibility");
            }
        }
        private Visibility payVisibility = Visibility.Collapsed;
        public Visibility PayVisibility
        {
            get { return payVisibility; }
            set
            {
                payVisibility = value;
                NotifyPropertyChanged("PayVisibility");
            }
        }
        private Visibility previousVisibility = Visibility.Collapsed;
        public Visibility PreviousVisibility
        {
            get { return previousVisibility; }
            set
            {
                previousVisibility = value;
                NotifyPropertyChanged("PreviousVisibility");
            }
        }
        private Visibility nextVisibility = Visibility.Collapsed;
        public Visibility NextVisibility
        {
            get { return nextVisibility; }
            set
            {
                nextVisibility = value;
                NotifyPropertyChanged("NextVisibility");
            }
        }
        //触摸快进是否显示
        private Visibility valueChangeVisibility = Visibility.Collapsed;
        public Visibility ValueChangeVisibility
        {
            get { return valueChangeVisibility; }
            set
            {
                valueChangeVisibility = value;
                NotifyPropertyChanged("ValueChangeVisibility");
            }
        }
        //触摸快进是否显示
        private Visibility volumeChangeVisibility = Visibility.Collapsed;
        public Visibility VolumeChangeVisibility
        {
            get { return volumeChangeVisibility; }
            set
            {
                volumeChangeVisibility = value;
                NotifyPropertyChanged("VolumeChangeVisibility");
            }
        }
        private Visibility moreVideoVisibility = Visibility.Collapsed;
         public Visibility MoreVideoVisibility
        {
            get { return moreVideoVisibility; }
            set
            {
                moreVideoVisibility = value;
                NotifyPropertyChanged("MoreVideoVisibility");
            }
        }
         private Visibility moreCommentVisibility = Visibility.Collapsed;
         public Visibility MoreCommentVisibility
         {
             get { return moreCommentVisibility; }
             set
             {
                 moreCommentVisibility = value;
                 NotifyPropertyChanged("MoreCommentVisibility");
             }
         }
        private double volume = 1;
        public double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                NotifyPropertyChanged("Volume");
            }
        }
        private string errMsg;
        public string ErrMsg
        {
            get { return errMsg; }
            set
            {
                errMsg = value;
                NotifyPropertyChanged("ErrMsg");
            }
        }

        private string valueChangeMsg;
        public string ValueChangeMsg
        {
            get { return valueChangeMsg; }
            set
            {
                valueChangeMsg = value;
                NotifyPropertyChanged("ValueChangeMsg");
            }
        }
        private string videoId;
        public string VideoId
        {
            get { return videoId; }
            set
            {
                videoId = value;
            }
        }
        private string currentDefinitionName;
        public string CurrentDefinitionName
        {
            get { return currentDefinitionName; }
            set
            {
                currentDefinitionName = value;
                NotifyPropertyChanged("CurrentDefinitionName");
            }
        }
        private bool isChangeDefinition;
        public bool IsChangeDefinition
        {
            get
            {
                return isChangeDefinition;
            }
            set
            {
                isChangeDefinition = value;
            }
        }
        private int dramaPageCount = 1;
        public int DramaPageCount
        {
            get { return dramaPageCount; }
            set { dramaPageCount = value; }
        }
        public void LoadedDramaItem()
        {
            string videoListUrl = CommonData.GetVideoListUrl + "&videoId=" + VideoId + "&pageCount=" + DramaPageCount;
            System.Diagnostics.Debug.WriteLine("获取剧集列表 url：" + videoListUrl);
            HttpHelper.httpGet(videoListUrl, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (!string.IsNullOrEmpty(result))
                {
                    VideoInfoResult videosResult = null;
                    try
                    {
                        videosResult = JsonConvert.DeserializeObject<VideoInfoResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                    }

                    if (videosResult == null)
                    {
                        JsonError(result);
                    }
                    else if (videosResult.err_code == HttpHelper.rightCode)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                if( videosResult.data != null && videosResult.data.Count > 0){
                                
                                    if (DramaPageCount > 1)
                                    {
                                        foreach (VideoInfo video in videosResult.data)
                                        {
                                            AllDramas.Add(video);
                                        }
                                        if (VideoStyleType == "1")
                                        {
                                            App.PlayerModel.MoreVideoVisibility = Visibility.Collapsed;
                                        }
                                        else if (VideoStyleType == "2")
                                        {
                                            if (AllDramas.Count == 30 * DramaPageCount)
                                            {
                                                App.PlayerModel.MoreVideoVisibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                App.PlayerModel.MoreVideoVisibility = Visibility.Collapsed;
                                            }
                                        }
                                        dramaPageCount++;
                                    }
                                    else
                                    {
                                        AllDramas = videosResult.data;
                                        PlayerInfo player = CallbackManager.currentPage as PlayerInfo;
                                        if (player != null)
                                        {
                                            player.LoadDramaSeletedItem(videoId);
                                        }
                                    }
                                }else {
                                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                                    {
                                        App.HideLoading();
                                        if (dramaPageCount == 1)
                                        {
                                            App.ShowToast("视频列表为空");
                                            PlayerInfo player = CallbackManager.currentPage as PlayerInfo;

                                            if (player != null && player.NavigationService.CanGoBack)
                                            {
                                                player.NavigationService.GoBack();
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    }
                    
                }
                else
                {
                    if (CommonData.NetworkStatus != "None")
                    {
                        LoadedDramaItem();
                    }
                    else {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                App.HideLoading();
                                App.ShowToast("获取视频列表失败,没有可用网络");
                                PlayerInfo player = CallbackManager.currentPage as PlayerInfo;
                                if (player != null && player.NavigationService.CanGoBack)
                                {
                                    player.NavigationService.GoBack();
                                }
                            });
                    }
                }
            });
        }
        private int _commentPageCount = 1;
        public int CommentPageCount
        {
            get { return _commentPageCount; }
            set
            {
                if (_commentPageCount != value)
                {
                    _commentPageCount = value;
                    //LoadedComment();
                }
            }
        }
        internal void LoadedComment()
        {
            string commentUrl = commentUrl = CommonData.GetMoreVideoComment + "&videoId=" + VideoId + "&pageCount=" + CommentPageCount;
            System.Diagnostics.Debug.WriteLine("获取剧集评论 url：" + commentUrl);
            HttpHelper.httpGet(commentUrl, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    CommentResult commentResult = null;
                    try
                    {
                        commentResult = JsonConvert.DeserializeObject<CommentResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        App.JsonError(result);
                    }
                    if (commentResult == null)
                    {
                        JsonError(result);
                    }
                    else if (commentResult.err_code == HttpHelper.rightCode && commentResult.data.Count > 0)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                if (CommentPageCount == 1)
                                {
                                    Comments = commentResult.data;
                                }
                                else if (CommentPageCount > 1)
                                {
                                    Comments.AddRange(commentResult.data);
                                }
                                if (Comments.Count == 30 * CommentPageCount)
                                {
                                    MoreCommentVisibility = Visibility.Visible;
                                }
                                else
                                {
                                    MoreCommentVisibility = Visibility.Collapsed;
                                }
                                _commentPageCount++;
                            });
                        }
                    }
                }
                else
                {
                    //App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }

        internal void LoadedDetail()
        {
            System.Diagnostics.Debug.WriteLine("获取剧集详情 url：" + CommonData.GetVideoDetailUrl + "&videoId=" + VideoId);
            HttpHelper.httpGet(CommonData.GetVideoDetailUrl + "&videoId=" + VideoId, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    VideoDetailResult videosDetailResult = null;
                    try
                    {
                        videosDetailResult = JsonConvert.DeserializeObject<VideoDetailResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        App.JsonError(result);
                    }
                    if (videosDetailResult == null)
                    {
                        JsonError(result);
                    }
                    else if (videosDetailResult.err_code == HttpHelper.rightCode && videosDetailResult.data != null && videosDetailResult.data.detail != null)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                VideoDetail = videosDetailResult.data.detail;
                                VideoName = videosDetailResult.data.detail.name;
                                VideoStyleType = videosDetailResult.data.detail.typeId;
                                if (VideoStyleType == "1")
                                {
                                    App.PlayerModel.MoreVideoVisibility = Visibility.Collapsed;
                                }
                                else if (VideoStyleType == "2")
                                {
                                    if (AllDramas.Count == 30 * DramaPageCount)
                                    {
                                        App.PlayerModel.MoreVideoVisibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        App.PlayerModel.MoreVideoVisibility = Visibility.Collapsed;
                                    }
                                }
                                dramaPageCount++;
                            });
                        }
                    }
                }
                else
                {
                    // App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }
        public void LoadRrelatedVideo()
        {
            System.Diagnostics.Debug.WriteLine("获取剧集花絮 url：" + CommonData.GetVideoRrelated + "&videoId=" + VideoId);
            HttpHelper.httpGet(CommonData.GetVideoRrelated + "&videoId=" + VideoId, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    VideoInfoResult videosResult = null;
                    try
                    {
                        videosResult = JsonConvert.DeserializeObject<VideoInfoResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        App.JsonError(result);
                    }
                    if (videosResult == null)
                    {
                        JsonError(result);
                    }
                    else if (videosResult.err_code == HttpHelper.rightCode)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                AllRelateds = videosResult.data;
                            });
                        }
                    }
                }
                else
                {
                    //App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }
        public bool isM3U8Video = false;
        public VideoInfo currentVideo;
        public void PlayerVideo(VideoInfo info)
        {
            currentVideo = info;
            //设置多媒体控件的网络视频资源
            if (info.downloadUrl.Count > 0)
            {
                isM3U8Video = false;
                App.PlayerModel.PayVisibility = Visibility.Collapsed;
                App.PlayerModel.LoadVisibility = Visibility.Visible;
                int downIndex = 0;
                for (int i = 0; i < info.downloadUrl.Count; i++)
                {
                    if (info.downloadUrl[i].name == "高清")
                    {
                        downIndex = i;
                        break;
                    }
                }
                VideoDefinition Definition = info.downloadUrl[downIndex];
                GetVideoSource(Definition, info);
            }
            else
            {
                //尝试获取数据源
                string playSourceUrl = CommonData.GetVideoResourceUrl + "&videoId=" + info.videoId;
                System.Diagnostics.Debug.WriteLine("获取播放源2：" + playSourceUrl);
                HttpHelper.httpGet(playSourceUrl, (ar) =>
                {
                    string result = HttpHelper.SyncResultTostring(ar);
                    if (result != null)
                    {
                        VideoDetailResult videosDetailResult = null;
                        try
                        {
                            videosDetailResult = JsonConvert.DeserializeObject<VideoDetailResult>(result);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                            App.JsonError(result);
                        }
                        if (videosDetailResult == null)
                        {
                            JsonError(result);
                        }
                        else if (videosDetailResult != null && videosDetailResult.err_code == HttpHelper.rightCode && videosDetailResult.data != null && videosDetailResult.data.detail != null)
                        {
                            if (CallbackManager.currentPage != null)
                            {
                                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                                {
                                    //todo
                                    App.HideLoading();
                                    LoadVisibility = Visibility.Collapsed;
                                    //ErrMsg = "格式不支持";
                                    //PayVisibility = Visibility.Visible;

                                    //info.downloadUrl = videosDetailResult.data.downloadUrl;
                                    //info.downloadUrl = videosDetailResult.data.videoSources;
                                    //currentVideo.downloadUrl = videosDetailResult.data.videoSources;
                                    PlayerM3U8Video(videosDetailResult.data.videoSources);
                                });
                            }
                        }
                        else 
                        {
                            if (CallbackManager.currentPage != null)
                            {
                                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                                {
                                    App.HideLoading();
                                    LoadVisibility = Visibility.Collapsed;
                                    ErrMsg = videosDetailResult.err_msg;
                                    PayVisibility = Visibility.Visible;
                                });
                            }
                        }
                    }
                    else
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                App.HideLoading();
                                LoadVisibility = Visibility.Collapsed;
                                // App.ShowToast("获取数据失败，请检查网络或重试");
                            });
                        }
                    }
                });
            }
        }
        private string currentDefinitionUrl;
        public void GetVideoSource(VideoDefinition definition, VideoInfo info)
        {
            if (definition == null || string.IsNullOrEmpty(definition.url) || currentDefinitionUrl == definition.url) return;
            currentDefinitionUrl = definition.url;
            CurrentDefinitionName = definition.name;
            System.Diagnostics.Debug.WriteLine("获取播放源：" + definition.url);
            HttpHelper.httpGet(definition.url, (ar) =>
            {
                App.HideLoading();
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
                        App.JsonError(result);
                    }
                    if (videosResult == null)
                    {
                        JsonError(result);
                    }
                    else if (videosResult.status == "ok" && videosResult.info != null)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                MediaSource = new Uri(videosResult.info, UriKind.RelativeOrAbsolute);
                                App.MainViewModel.AddRememberVideo(info);
                                CurrentDefinitionName = definition.name;
                                // VideoDownloadUrl = info.downloadUrl;
                                System.Diagnostics.Debug.WriteLine("视频地址 ： " + videosResult.info);
                            });
                        }
                    }
                }
                //else
                //{
                //    App.ShowToast("获取视频数据失败，请检查网络或重试");
                //}
            });
        }
        private void PlayerM3U8Video(List<VideoDefinition> listVideos)
        {
            if (listVideos.Count == 0) return;
            isM3U8Video = true;
            int downIndex = 0;
            for (int i = 0; i < listVideos.Count; i++)
            {
                if (listVideos[i].name == "高清")
                {
                    downIndex = i;
                    break;
                }
            }
            VideoDefinition Definition = listVideos[downIndex];
            GetM3U8VideoSource(Definition, currentVideo);
        }
        public void GetM3U8VideoSource(VideoDefinition definition, VideoInfo info)
        {
            if (definition == null || string.IsNullOrEmpty(definition.url) || currentDefinitionUrl == definition.url) return;
            currentDefinitionUrl = definition.url;
            CurrentDefinitionName = definition.name;
            System.Diagnostics.Debug.WriteLine("获取播放源：" + definition.url);
            HttpHelper.httpGet(definition.url, (ar) =>
            {
                App.HideLoading();
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
                        App.JsonError(result);
                    }
                    if (videosResult == null)
                    {
                        JsonError(result);
                    }
                    else if (videosResult.status == "ok" && videosResult.info != null)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                               PlayerInfo playpage =  CallbackManager.currentPage as PlayerInfo;
                               if (playpage != null) {
                                   playpage.BeginPlayM3u8Video(videosResult.info);
                                   App.MainViewModel.AddRememberVideo(info);
                                   CurrentDefinitionName = definition.name;
                                   System.Diagnostics.Debug.WriteLine("视频地址 ： " + videosResult.info);
                               }
                                
                            });
                        }
                    }
                }
                //else
                //{
                //    App.ShowToast("获取视频数据失败，请检查网络或重试");
                //}
            });
        }
        public void JsonError(string result)
        {
            JsonError jsonError = null;
            try
            {
                jsonError = JsonConvert.DeserializeObject<JsonError>(result);
            }
                catch
            {
            }
            if (jsonError != null && !string.IsNullOrEmpty(jsonError.err_msg))
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    App.HideLoading();
                    App.ShowToast(jsonError.err_msg);
                    //App.JsonError(result);
                    LoadVisibility = Visibility.Collapsed;
                    PayVisibility = Visibility.Visible;
                    ErrMsg = jsonError.err_msg;
                });
            }
        }

        internal void ReloadNewVideo()
        {
            LoadedDetail();
            LoadedComment();
            LoadRrelatedVideo();
        }
        private string videoStyleType = "2";
        public string VideoStyleType
        {
            get 
            {
                return videoStyleType; 
            }
            set
            {
                videoStyleType = value;
                if (videoStyleType == "1")
                {
                    if (CallbackManager.currentPage != null)
                    {
                        VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle"] as Style;
                    }
                }
                else if (videoStyleType == "2")
                {
                    if (CallbackManager.currentPage != null)
                    {
                        VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle2"] as Style;
                    }
                }
            }
        }
        private Style multipleVideoStyle = null;
        public Style MultipleVideoStyle
        {
            get
            {
                if (VideoStyleType == "1")
                {
                    if (CallbackManager.currentPage != null)
                    {
                        multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
                    }
                }
                else if (VideoStyleType == "2")
                {
                    if (CallbackManager.currentPage != null)
                    {
                        multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle3"] as Style;
                    }
                }
                else if (VideoStyleType == null)
                {
                    if (CallbackManager.currentPage != null)
                    {
                        multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
                    }
                }
                return multipleVideoStyle;
            }
            set
            {
                multipleVideoStyle = value;
                NotifyPropertyChanged("MultipleVideoStyle");
            }
        }
        private Style videoStyle = null;
        public Style VideoStyle
        {
            get
            {
                if (videoStyle == null)
                {
                    if (videoStyleType == "1")
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle"] as Style;
                        }
                    }
                    else if (videoStyleType == "2")
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle2"] as Style;
                        }
                    }
                }
               
                return videoStyle;
            }
            set
            {
                videoStyle = value;
                NotifyPropertyChanged("VideoStyle");
            }
        }


        internal void ClearData()
        {
            AllDramas = null;
            VideoStyle = null;
            Comments = null;
            MediaSource = null;
            VideoDetail = null;
            AllRelateds = null;
            VideoDownloadUrl = null;
            currentDefinitionUrl = null;
            MoreCommentVisibility = Visibility.Collapsed;
            MoreCommentVisibility = Visibility.Collapsed;
            DramaPageCount = 1;
            CommentPageCount = 1;
        }

    }
}
