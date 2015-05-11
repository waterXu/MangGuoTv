using MangGuoTv.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class DownVideoViewModel:ViewModelBase
    {
        private string downingIdsIso = "DowndingIds";
        private string downedIdsIso = "DownedIds";
        private string downingVideosIso = "DownVideos/DowningVideos.dat";
        private string downedVideosIso = "DownVideos/DownedVideos.dat";
        private bool isDownding = false;
        private WebClient webClient;
        public DownVideoViewModel()
        {
            //todo  加载本地的数据
            this.DowningVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            this.DownedVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            loadLocalVideoData();
            if (this.DowningVideo.Count > 0) 
            {
                isDownding = true;
                BeginDownVideos();
            }
            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownVideoProgressChanged);
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownVideoCompleted);
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
            sw = Stopwatch.StartNew();//用来记录总的下载时间
            sw1 = Stopwatch.StartNew();//用来记录下载过程中的时间片，用于计算临时速度
            currentDownVideo = DowningVideo[0];
            System.Diagnostics.Debug.WriteLine("视频地址：" + currentDownVideo.DownUrl);
            webClient.DownloadStringAsync(new Uri(currentDownVideo.DownUrl));
            currentDownVideo.IsLoading = true;
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
            currentDownVideo.Speed = this.speed + "KB";
            currentDownVideo.LoadProgress = e.ProgressPercentage;
            currentDownVideo.Size = (e.TotalBytesToReceive/(1024 * 1024)).ToString();
            currentDownVideo.Loadsize = (e.BytesReceived / (1024 * 1024)).ToString();
            siz = e.TotalBytesToReceive;
            loadsiz = e.BytesReceived;
            sw1.Start();
        }
        private void DownVideoCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            sw.Stop();
            siz = siz / 1024;
            long num = siz / ((long)sw.Elapsed.Seconds);
            sw.Reset();
            currentDownVideo.IsLoading = false;
            currentDownVideo.IsLoaded = true;
            DowningVideo.Remove(currentDownVideo);
            DownedVideo.Add(currentDownVideo);
            DownedVideoids.Add(currentDownVideo.VideoIndex);
            DowningVideoids.Remove(currentDownVideo.VideoIndex);
            SaveVideoData();
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
            // todo 
            video.DownUrl = videoInfo.downloadUrl[0].url;
            DowningVideo.Add(video);
            DowningVideoids.Add(video.VideoId);
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
                BeginDownVideos();
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
