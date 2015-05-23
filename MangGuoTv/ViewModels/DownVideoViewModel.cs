using MangGuoTv.Models;
using Microsoft.Phone.Info;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class DownVideoViewModel:ViewModelBase
    {
        private string downingIdsIso { get { return "DowndingIds"; } }
        private string downedIdsIso { get { return "DownedIds"; } }
        private string downingVideosIso { get { return CommonData.IsoRootPath +"DowningVideos.dat"; } }
        private string downedVideosIso { get { return CommonData.IsoRootPath + "DownedVideos.dat"; } }
       // private string videoSavePath { get { return "DownVideos\\"; } }

        public bool isDownding = false;
        private WebClient webClient;
        public DownVideoViewModel()
        {
            this.DowningVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            this.DownedVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            webClient = new WebClient();
        }
        public void CheckLocalData() 
        {
            //加载本地的数据
            loadLocalVideoData();
            if (this.DowningVideo.Count > 0)
            {
                BeginDownVideos();
            }
            //webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(LoadedCompleted);
        }
        public void loadLocalVideoData()
        {
            //加载正在下载视频数据
            string loadingVideos = WpStorage.ReadIsolatedStorageFile(downingVideosIso);
            if (!string.IsNullOrEmpty(loadingVideos))
            {
                DowningVideo = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(loadingVideos);
                foreach (DownVideoInfoViewMoel video in DowningVideo)
                {
                    DowningVideoids.Add(video.VideoId);
                }
            }
            else
            {
                DowningVideo = null;
            }
            //加载完成下载视频数据
            string loadedVideos = WpStorage.ReadIsolatedStorageFile(downedVideosIso);
            if (!string.IsNullOrEmpty(loadedVideos))
            {
                DownedVideo = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(loadedVideos);
                foreach (DownVideoInfoViewMoel video in DownedVideo)
                {
                    DownedVideoids.Add(video.VideoId);
                }
            }
            else
            {
                DownedVideo = null;
            }

            ////加载正在下载视频的id列表
            //if (WpStorage.GetIsoSetting(downingIdsIso) != null)
            //{
            //    string downingIds = WpStorage.GetIsoSetting(downingIdsIso).ToString();
            //    downingVideoids = JsonConvert.DeserializeObject<HashSet<string>>(downingIds);
            //}
            //else
            //{
            //    downingVideoids = null;
            //}
            ////加载下载视频的id列表
            //if (WpStorage.GetIsoSetting(downedIdsIso) != null)
            //{
            //    string downedIds = WpStorage.GetIsoSetting(downedIdsIso).ToString();
            //    downedVideoids = JsonConvert.DeserializeObject<HashSet<string>>(downedIds);
            //}
            //else
            //{
            //    downedVideoids = null;
            //}
        }
        public DownVideoInfoViewMoel currentDownVideo = null;
        public void BeginDownVideos()
        {
            if (CommonData.NetworkStatus != "WiFi") return;
            if (isDownding || DowningVideo.Count==0) return;
            isDownding = true;
            currentDownVideo = DowningVideo[0];
            System.Diagnostics.Debug.WriteLine("视频地址：" + currentDownVideo.DownUrl);
            HttpHelper.httpGet(currentDownVideo.DownUrl, (ar) =>
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
                    if (videosResult != null && videosResult.status == "ok" && videosResult.info != null)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            currentDownVideo.IsLoading = true;
                            currentDownVideo.IsLoadError = false;
                            HttpHelper.httpGet(currentDownVideo.Image, (imageAr) =>
                            {
                                byte[] imgdata = HttpHelper.SyncResultToByte(imageAr);
                                if (imgdata != null)
                                {
                                    string imageType = currentDownVideo.Image.Remove(0, currentDownVideo.Image.Length - 4);
                                    WpStorage.SaveFilesToIsoStore(CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + imageType, imgdata);
                                    currentDownVideo.LocalImage = CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + imageType;
                                }
                            });
                            //sw = Stopwatch.StartNew();//用来记录总的下载时间
                            //sw1 = Stopwatch.StartNew();//用来记录下载过程中的时间片，用于计算临时速度
                            webClient = new WebClient();
                            webClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(DownVideoProgressChanged);
                            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownVideoProgressChanged);
                            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(LoadedCompleted);
                            webClient.OpenReadAsync(new Uri(videosResult.info));
                        });
                    }
                }
                else
                {
                    //todo
                }
            });
        }
        public void StopDownVideo() 
        {
            webClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(DownVideoProgressChanged);
            if (webClient.IsBusy) 
            {
                webClient.CancelAsync();
                currentDownVideo.IsLoadError = true;
                currentDownVideo.IsLoading = false;
            }
            isDownding = false;
        }
        private long siz;
        private long speed;
        private long loadsiz; 
        //private Stopwatch sw;
        //private Stopwatch sw1;
        private void DownVideoProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //sw1.Stop();
            //long num = e.BytesReceived / 1024;
            //if (sw1.Elapsed.Seconds != 0)
            //{
            //    speed = num / ((long)sw1.Elapsed.Seconds);
            //}
            if (CommonData.NetworkStatus != "WiFi") 
            {
                StopDownVideo();
                return;
            }
            if (CallbackManager.currentPage != null) 
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    currentDownVideo.LoadProgress = e.ProgressPercentage;
                    currentDownVideo.Size = Convert.ToDouble((double)((e.TotalBytesToReceive) / (double)((double)1024 * (double)1024))).ToString("0.00") + "MB"; 
                    currentDownVideo.Loadsize = Convert.ToDouble((double)((e.BytesReceived) / (double)((double)1024 * (double)1024))).ToString("0.00") + "MB"; 
                });
            }
          
            siz = e.TotalBytesToReceive;
            loadsiz = e.BytesReceived;
           // sw1.Start();
        }

        private void LoadedCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //sw.Stop();
           // siz = siz / 1024;
            //long num = siz / ((long)sw.Elapsed.Seconds);
            //sw.Reset();

            if (e.Error != null)
            {
                System.Diagnostics.Debug.WriteLine("wc_DownloadStringCompleted  error：" + e.Error);
                 CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    currentDownVideo.IsLoadError = true;
                    currentDownVideo.IsLoading = false;
                    isDownding = false;
                    //重新下载
                    BeginDownVideos();
                });
                //todo  按win键或长按回退  导致
                return;
            }
            try
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    WpStorage.SaveFilesToIsoStore(CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4", e.Result);
                    currentDownVideo.IsLoading = false;
                    currentDownVideo.IsLoaded = true;
                    currentDownVideo.LocalDownloadUrl = CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4";
                    DownedVideo.Add(currentDownVideo);
                    DownedVideoids.Add(currentDownVideo.VideoIndex);
                    DowningVideo.RemoveAt(0);
                    DowningVideoids.Remove(currentDownVideo.VideoIndex);
                    SaveVideoData();
                    if (App.MainViewModel.NeedDownedTip)
                    {
                        App.ShowToast( currentDownVideo.name+ "  下载完成");
                    }
                    //进行下一个下载
                    isDownding = false;
                    BeginDownVideos();
                });

            }
            catch
            {

            }

        }
        public void SaveVideoData() 
        {
            //string downingVideoidData = null;
            //if (DowningVideoids != null && DowningVideoids.Count > 0)
            //{
            //    //把hashset表反序列化为字符串 存入独立存储
            //    downingVideoidData = JsonConvert.SerializeObject(DowningVideoids);
            //}
            //WpStorage.SetIsoSetting(downingIdsIso, downingVideoidData);
            //string downedVideoidData = null;
            //if (DownedVideoids != null && DownedVideoids.Count > 0)
            //{
            //    //把hashset表反序列化为字符串 存入独立存储
            //    downedVideoidData = JsonConvert.SerializeObject(DownedVideoids);
            //}
            //WpStorage.SetIsoSetting(downedIdsIso, downedVideoidData);

            string downingVideosData = null;
            if (DowningVideo != null &&　DowningVideo.Count > 0)
            {
                //把正在下载的列表反序列化为字符串 存入独立存储
                downingVideosData = JsonConvert.SerializeObject(DowningVideo);
            }
            WpStorage.SaveStringToIsoStore(downingVideosIso, downingVideosData);

            string downedVideosData = null;
            if (DownedVideo != null && DownedVideo.Count > 0)
            {
                //把已经下载的列表反序列化为字符串 存入独立存储
                downedVideosData = JsonConvert.SerializeObject(DownedVideo);
            }
            WpStorage.SaveStringToIsoStore(downedVideosIso, downedVideosData);
            UpdateIsoSize();
        }

        public void AddDownVideo(VideoInfo videoInfo) 
        {
            if (videoInfo.downloadUrl!=null && videoInfo.downloadUrl.Count == 0)
            {
                App.ShowToast(videoInfo.name+" 没有可用下载链接");
                return;
            }
            DownVideoInfoViewMoel video = new DownVideoInfoViewMoel();
            video.Name = videoInfo.name;
            video.Image = videoInfo.image;
            video.Time = videoInfo.time;
            video.VideoId = videoInfo.videoId;
            video.VideoIndex = videoInfo.videoIndex;
            video.Desc = videoInfo.desc;
            video.IsLoading = false;
            video.IsLoaded = false;
            int downIndex = 0;
            for(int i=0;i<videoInfo.downloadUrl.Count;i++){
                if (videoInfo.downloadUrl[i].name == "高清")
                {
                    downIndex = i;
                    break;
                }
            }
            video.DownUrl = videoInfo.downloadUrl[downIndex].url;
            DowningVideo.Add(video);
            DowningVideoids.Add(video.VideoId);
            SaveVideoData();
            BeginDownVideos();
        }
        #region Property
        private HashSet<string> downingVideoids;
        public HashSet<string> DowningVideoids 
        {
            get 
            {
                if (downingVideoids == null) 
                {
                    downingVideoids = new HashSet<string>();
                }
                return downingVideoids;
            }
            set
            {
                downingVideoids = value;

            }
        }
        private HashSet<string> downedVideoids;
        public HashSet<string> DownedVideoids
        {
            get
            {
                if (downedVideoids == null)
                {
                    downedVideoids = new HashSet<string>();
                }
                return downedVideoids;
            }
            set
            {
                downedVideoids = value;
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> downingVideo;
        public ObservableCollection<DownVideoInfoViewMoel> DowningVideo 
        {
            get 
            {
                if (downingVideo == null)
                {
                    downingVideo = new ObservableCollection<DownVideoInfoViewMoel>();
                }
                return downingVideo;
            }
            set
            { 
                downingVideo = value;
                NotifyPropertyChanged("DowningVideo");
                //if (!isDownding) isDownding = true;
                //BeginDownVideos();
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> downedVideo { get; set; }
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
        private string isoSize;
        public string IsoSize 
        {
            get 
            {
                isoSize = Convert.ToDouble((double)((WpStorage.isoFile.AvailableFreeSpace) / (double)((double)1024 * (double)1024 * (double)1024))).ToString("0.00") + "GB";  //获取独立存储的可用空间大小
                return isoSize;
            }
            set 
            {
                isoSize = value;
                NotifyPropertyChanged("IsoSize");
            }
        }
        public void UpdateIsoSize() 
        {
            IsoSize = Convert.ToDouble((double)((WpStorage.isoFile.AvailableFreeSpace) / (double)((double)1024 * (double)1024 * (double)1024))).ToString("0.00") + "GB";  //获取独立存储的可用空间大小
        }
        #endregion

    }
}
