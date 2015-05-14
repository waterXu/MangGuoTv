using MangGuoTv.Models;
using MangGuoTv.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MangGuoTv.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private string _videoName;
        public string VideoName
        {
            get { return _videoName; }
            set {
                if (_videoName != value)
                {
                    _videoName = value;
                    NotifyPropertyChanged("VideoName");
                }
            }
        }
        private List<VideoInfo> _AllDramas;
        public List<VideoInfo> AllDramas
        {
            get 
            {
                if (_AllDramas == null) 
                {
                    _AllDramas = new List<VideoInfo>();
                }
                return _AllDramas;
            }
            set
            {
                _AllDramas = value;
                NotifyPropertyChanged("AllDramas");
            }
        }
        private List<VideoInfo> _AllRelateds;
        public List<VideoInfo> AllRelateds
        {
            get 
            {
                if (_AllRelateds == null) 
                {
                    _AllRelateds = new List<VideoInfo>();
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
                _downloadUrl = value;
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
        private string videoId;
        public string VideoId
        {
            get { return videoId; }
            set
            {
                videoId = value;
            }
        }
        private int dramaPageCount = 0;
        public int DramaPageCount 
        {
            get { return dramaPageCount; }
            set { dramaPageCount = value; }
        }
        public void LoadedDramaItem() 
        {
            string videoListUrl =  CommonData.GetVideoListUrl + "&videoId=" + VideoId + "&pageCount=" + DramaPageCount;
            System.Diagnostics.Debug.WriteLine("获取剧集列表 url：" + videoListUrl);
            HttpHelper.httpGet(videoListUrl, (ar) =>
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
                    }

                    if (videosResult == null) 
                    {
                        JsonError(result);
                    }
                    else if ( videosResult.err_code == HttpHelper.rightCode)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            if (DramaPageCount > 0)
                            {
                                AllDramas.AddRange(videosResult.data);
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
                        });
                    }
                }
                else
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }
        private int _commentPageCount = 0;
        public int CommentPageCount 
        {
            get { return _commentPageCount; }
            set 
            {
                if (_commentPageCount != value) 
                {
                    CommentPageCount = value;
                    LoadedComment();
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
                    else if ( commentResult.err_code == HttpHelper.rightCode && commentResult.data.Count > 0)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            if (CommentPageCount == 0)
                            {
                                Comments = commentResult.data;
                            }
                            else if (CommentPageCount > 0)
                            {
                                Comments.AddRange(commentResult.data);
                            }
                        });
                    }
                }
                else
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
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
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            VideoDetail = videosDetailResult.data.detail;
                            VideoName = videosDetailResult.data.detail.name;
                            VideoStyleType = videosDetailResult.data.detail.typeId;
                            //VideoSources = videosDetailResult.data.videoSources;
                            //VideoDownloadUrl = videosDetailResult.data.downloadUrl; (CallbackManager.currentPage as PlayerInfo).PlayerVideo();
                        });
                    }
                }
                else
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
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
                    else if ( videosResult.err_code == HttpHelper.rightCode)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            AllRelateds = videosResult.data;
                        });
                    }
                }
                else
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }
        public void PlayerVideo(VideoInfo info)
        {
            //设置多媒体控件的网络视频资源
            if (info.downloadUrl.Count > 0)
            {
                App.PlayerModel.PayVisibility = Visibility.Collapsed;
                App.PlayerModel.LoadVisibility = Visibility.Visible;
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
                            App.JsonError(result);
                        }
                        if (videosResult == null)
                        {
                            JsonError(result);
                        }
                        else if ( videosResult.status == "ok" && videosResult.info != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                MediaSource = new Uri(videosResult.info, UriKind.RelativeOrAbsolute);
                                App.MainViewModel.AddRememberVideo(info);
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
            else 
            {
                //尝试获取数据源
                string playSourceUrl = CommonData.GetVideoResourceUrl + "&videoId="+info.videoId;
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
                        else if (videosDetailResult.err_code == HttpHelper.rightCode && videosDetailResult.data != null && videosDetailResult.data.detail != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                //info.downloadUrl = videosDetailResult.data.downloadUrl;
                                info.downloadUrl = videosDetailResult.data.videoSources;
                                PlayerM3U8Video(videosDetailResult.data.videoSources);
                            });
                        }
                        else if (videosDetailResult != null && videosDetailResult.err_code != HttpHelper.rightCode)
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
                    else
                    {
                        App.HideLoading();
                        LoadVisibility = Visibility.Collapsed;
                        App.ShowToast("获取数据失败，请检查网络或重试");
                    }
                });
            }
        }
        public  void JsonError(string result)
        {
            try
            {
                JsonError jsonError = JsonConvert.DeserializeObject<JsonError>(result);
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    App.HideLoading();
                    App.ShowToast(jsonError.err_msg);
                    App.JsonError(result);
                    LoadVisibility = Visibility.Collapsed;
                    PayVisibility = Visibility.Visible;
                    ErrMsg = jsonError.err_msg;
                });
            }
            catch 
            { 

            }

        }
        private void PlayerM3U8Video(List<VideoDefinition> list)
        {

        }
        internal void ReloadNewVideo()
        {
            LoadedDetail();
            LoadedComment();
            LoadRrelatedVideo();
        }
        private string videoStyleType;
        public string VideoStyleType 
        {
            get { return videoStyleType; }
            set 
            {
                videoStyleType = value;
                if (videoStyleType == "1") 
                {
                    VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle"] as Style;
                }
                else if (videoStyleType == "2") 
                {
                    VideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle2"] as Style;
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
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
                }
                else if (VideoStyleType == "2")
                {
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle3"] as Style;
                }
                else if (VideoStyleType == null)
                {
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
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
        }
    }
}
