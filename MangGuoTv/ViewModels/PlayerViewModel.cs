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
            get { return _AllDramas; }
            set
            {
                _AllDramas = value;
                NotifyPropertyChanged("AllDramas");
            }
        }
        private List<VideoInfo> _AllRelateds;
        public List<VideoInfo> AllRelateds
        {
            get { return _AllRelateds; }
            set
            {
                _AllRelateds = value;
                NotifyPropertyChanged("AllRelateds");
            }
        }
        private List<Comment> _comments;
        public List<Comment> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                NotifyPropertyChanged("Comments");
            }
        }
        private VideoDetail _videoDetail;
        public VideoDetail VideoDetail 
        {
            get { return _videoDetail; }
            set
            {
                _videoDetail = value;
                NotifyPropertyChanged("VideoDetail");
            }
        }
        private List<VideoDefinition> _videoSources;
        public List<VideoDefinition> VideoSources
        {
            get { return _videoSources; }
            set
            {
                _videoSources = value;
            }
        }
        private List<VideoDefinition> _downloadUrl;
        public List<VideoDefinition> VideoDownloadUrl
        {
            get { return _downloadUrl; }
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
                    if (videosResult != null && videosResult.err_code == HttpHelper.rightCode)
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
                                (CallbackManager.currentPage as PlayerInfo).LoadDramaSeletedItem(videoId);
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
                    }
                    if (commentResult != null && commentResult.err_code == HttpHelper.rightCode && commentResult.data.Count > 0)
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
                    }
                    if (videosDetailResult != null && videosDetailResult.err_code == HttpHelper.rightCode && videosDetailResult.data != null && videosDetailResult.data.detail != null)
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
                    }
                    if (videosResult != null && videosResult.err_code == HttpHelper.rightCode)
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
                if (VideoStyleType == null)
                {
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
                }
                else if (VideoStyleType == "1")
                {
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle1"] as Style;
                }
                else if (VideoStyleType == "2")
                {
                    multipleVideoStyle = CallbackManager.currentPage.Resources["VideoListItemStyle3"] as Style;
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
                    videoStyle = new Style();
                }
            
                return videoStyle;
            }
            set 
            {
                videoStyle = value;
                NotifyPropertyChanged("VideoStyle");
            }
        }
      
    }
}
