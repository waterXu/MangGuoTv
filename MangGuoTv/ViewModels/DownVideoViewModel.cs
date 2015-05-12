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
        private string downingVideosIso { get { return "DownVideos/DowningVideos.dat"; } }
        private string downedVideosIso { get { return "DownVideos/DownedVideos.dat"; } }
        private string videoSavePath { get { return "DownVideos\\"; } }

        private bool isDownding = false;
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
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownVideoProgressChanged);
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(LoadedCompleted);
        }
        public void loadLocalVideoData()
        {
            //加载正在下载视频数据
            string loadingVideos = WpStorage.ReadIsolatedStorageFile(downingVideosIso);
            if (!string.IsNullOrEmpty(loadingVideos))
            {
                DowningVideo = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(loadingVideos);
            }
            //加载完成下载视频数据
            string loadedVideos = WpStorage.ReadIsolatedStorageFile(downedVideosIso);
            if (!string.IsNullOrEmpty(loadedVideos))
            {
                DownedVideo = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(loadedVideos);
            }

            //加载正在下载视频的id列表
            if (WpStorage.GetIsoSetting(downingIdsIso) != null)
            {
                string downingIds = WpStorage.GetIsoSetting(downingIdsIso).ToString();
                downingVideoids = JsonConvert.DeserializeObject<HashSet<string>>(downingIds);
            }
            //加载下载视频的id列表
            if (WpStorage.GetIsoSetting(downedIdsIso) != null)
            {
                string downedIds = WpStorage.GetIsoSetting(downedIdsIso).ToString();
                downedVideoids = JsonConvert.DeserializeObject<HashSet<string>>(downedIds);
            }
        }
        DownVideoInfoViewMoel currentDownVideo = null;
        public void BeginDownVideos()
        {
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
                    }
                    if (videosResult != null && videosResult.status == "ok" && videosResult.info != null)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            currentDownVideo.IsLoading = true;
                        });
                      
                        HttpHelper.httpGet(currentDownVideo.Image, (imageAr) =>
                        {
                            byte[] imgdata = HttpHelper.SyncResultToByte(imageAr);
                            if (imgdata != null)
                            {
                                string imageType = currentDownVideo.Image.Remove(0, currentDownVideo.Image.Length - 4);
                                WpStorage.SaveFilesToIsoStore(videoSavePath + currentDownVideo.VideoId.ToString() + imageType, imgdata);
                                currentDownVideo.LocalImage = videoSavePath + currentDownVideo.VideoId.ToString() + imageType;
                            }
                        });
                        sw = Stopwatch.StartNew();//用来记录总的下载时间
                        sw1 = Stopwatch.StartNew();//用来记录下载过程中的时间片，用于计算临时速度
                        webClient.OpenReadAsync(new Uri(videosResult.info));
                    }
                }
                else
                {
                    //todo
                }
            });
        }

        private long siz;
        private long speed;
        private long loadsiz; 
        private Stopwatch sw;
        private Stopwatch sw1;
        private void DownVideoProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            sw1.Stop();
            long num = e.BytesReceived / 1024;
            if (sw1.Elapsed.Seconds != 0)
            {
                speed = num / ((long)sw1.Elapsed.Seconds);
            }
            if (CallbackManager.currentPage != null) 
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    currentDownVideo.LoadProgress = e.ProgressPercentage;
                    currentDownVideo.Size = (e.TotalBytesToReceive / (1024 * 1024)).ToString() + "MB";
                    currentDownVideo.Loadsize = (e.BytesReceived / (1024 * 1024)).ToString() + "MB";
                });
            }
          
            siz = e.TotalBytesToReceive;
            loadsiz = e.BytesReceived;
            sw1.Start();
        }

        private void LoadedCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("完成");
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
            sw.Stop();
            siz = siz / 1024;
            long num = siz / ((long)sw.Elapsed.Seconds);
            sw.Reset();

            if (e.Error != null)
            {
                System.Diagnostics.Debug.WriteLine("wc_DownloadStringCompleted  error：" + e.Error);
                 CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    currentDownVideo.IsLoadError = true;
                    currentDownVideo.IsLoading = false;
                });
                //todo
                return;
            }
           
            try
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    WpStorage.SaveFilesToIsoStore(videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4", e.Result);
                    currentDownVideo.IsLoading = false;
                    currentDownVideo.IsLoaded = true;
                    currentDownVideo.LocalDownloadUrl = videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4";
                    DowningVideo.RemoveAt(0);
                    DowningVideoids.Remove(currentDownVideo.VideoIndex);
                    DownedVideo.Add(currentDownVideo);
                    DownedVideoids.Add(currentDownVideo.VideoIndex);
                    SaveVideoData();
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
            string downingVideoidData = null;
            if (DowningVideoids.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                downingVideoidData = JsonConvert.SerializeObject(DowningVideoids);
            }
            WpStorage.SetIsoSetting(downingIdsIso, downingVideoidData);
            string downedVideoidData = null;
            if (DownedVideoids.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                downedVideoidData = JsonConvert.SerializeObject(DownedVideoids);
            }
            WpStorage.SetIsoSetting(downingIdsIso, downedVideoidData);

            string downingVideosData = null;
            if (DowningVideo.Count > 0)
            {
                //把正在下载的列表反序列化为字符串 存入独立存储
                downingVideosData = JsonConvert.SerializeObject(DowningVideo);
            }
            WpStorage.SaveStringToIsoStore(downingVideosIso, downingVideosData);

            string downedVideosData = null;
            if (DownedVideo.Count > 0)
            {
                //把已经下载的列表反序列化为字符串 存入独立存储
                downedVideosData = JsonConvert.SerializeObject(DownedVideo);
            }
            WpStorage.SaveStringToIsoStore(downedVideosIso, downedVideosData);
        }

        public void AddDownVideo(VideoInfo videoInfo) 
        {
            DownVideoInfoViewMoel video = new DownVideoInfoViewMoel();
            video.Name = videoInfo.name;
            video.Image = videoInfo.image;
            video.Time = videoInfo.time;
            video.VideoId = videoInfo.videoId;
            video.VideoIndex = videoInfo.videoIndex;
            video.Desc = videoInfo.desc;
            video.IsLoading = false;
            video.IsLoaded = false;
            // todo 
            video.DownUrl = videoInfo.downloadUrl[0].url;
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
            get { return downingVideo; }
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
            get { return downedVideo; }
            set
            {
                downedVideo = value;
                NotifyPropertyChanged("DownedVideo");
            }
        }
        #endregion

    }
}
