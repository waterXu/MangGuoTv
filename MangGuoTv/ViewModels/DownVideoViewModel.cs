using MangGuoTv.Models;
using Microsoft.Phone.Info;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private string currentDownvideoTmp { get { return CommonData.videoSavePath + "\\tmp\\tmp.mp4"; } }
       // private string videoSavePath { get { return "DownVideos\\"; } }

        public bool isDownding = false;
        private WebClient webClient;
        public DownVideoViewModel()
        {
            this.DowningVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            this.DownedVideo = new ObservableCollection<DownVideoInfoViewMoel>();
        }
        public void CheckLocalData() 
        {
            //加载本地的数据
            loadLocalVideoData();
            if (this.DowningVideo.Count > 0)
            {
                BeginDownVideos();
            }
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
                          
                            DownloadFile(videosResult.info);
                        });
                    }
                }
                else
                {
                    //todo
                }
            });
        }
        IsolatedStorageFileStream streamToWriteTo ;
        HttpWebRequest request;
        public void DownloadFile(string url)
        {
            request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.AllowReadStreamBuffering = false;
            request.BeginGetResponse(new AsyncCallback(GetVideoData), request);
            string fileName = CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4";
            Debug.WriteLine("文件路径："+fileName);
            if (WpStorage.isoFile.FileExists(fileName))
            {
                WpStorage.isoFile.DeleteFile(fileName);
            }
            string strBaseDir = string.Empty;
            string delimStr = "\\";
            char[] delimiter = delimStr.ToCharArray();
            string[] dirsPath = fileName.Split(delimiter);
            for (int i = 0; i < dirsPath.Length - 1; i++)
            {
                strBaseDir = System.IO.Path.Combine(strBaseDir, dirsPath[i]);
                WpStorage.isoFile.CreateDirectory(strBaseDir);
            }
            streamToWriteTo = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, WpStorage.isoFile);
        }

        public void StopGetVideoData() 
        {
            if(streamToWriteTo != null){
                streamToWriteTo.Close();
                streamToWriteTo.Dispose();
            }
            if (request != null)
            {
                request.Abort();
                currentDownVideo.IsLoadError = true;
                currentDownVideo.IsLoading = false;
            }
            isDownding = false;
        }
        private void GetVideoData(IAsyncResult result)
        {
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
            try
            {
                WebResponse response = ((HttpWebRequest)result.AsyncState).EndGetResponse(result);
                Stream stream = response.GetResponseStream();
                long totalValue = response.ContentLength;
                byte[] data = new byte[16 * 1024];
                int read;
                while ((read = stream.Read(data, 0, data.Length)) > 0)
                {
                    if (CommonData.NetworkStatus != "WiFi")
                    {
                        StopGetVideoData();
                        break;
                    }
                    if (streamToWriteTo.Length != 0)
                    {
                        if (CallbackManager.currentPage != null)
                        {
                            CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                            {
                                try
                                {
                                    if (streamToWriteTo.CanWrite)
                                    {
                                        currentDownVideo.LoadProgress = (double)streamToWriteTo.Length * 100 / (double)totalValue;
                                        currentDownVideo.Size = Convert.ToDouble((double)((totalValue) / (double)((double)1024 * (double)1024))).ToString("0.0") + "MB";
                                        currentDownVideo.Loadsize = Convert.ToDouble((double)((streamToWriteTo.Length) / (double)((double)1024 * (double)1024))).ToString("0.0") + "MB";

                                    }
                                }
                                catch(Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("currentDownVideo异常："+ex.Message);
                                }
                               
                            });
                        }
                    }
                    streamToWriteTo.Write(data, 0, read);
                }
                //判断是否已经下载完成
                if (totalValue == streamToWriteTo.Length)
                {
                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                    {
#if DEBUG
                        memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
                        memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
                        memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
                        System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
                        currentDownVideo.IsLoading = false;
                        currentDownVideo.IsLoaded = true;
                        currentDownVideo.LocalDownloadUrl = CommonData.videoSavePath + currentDownVideo.VideoId.ToString() + ".mp4";
                        DownedVideo.Add(currentDownVideo);
                        DownedVideoids.Add(currentDownVideo.VideoId);
                        DowningVideo.RemoveAt(0);
                        DowningVideoids.Remove(currentDownVideo.VideoId);
                        SaveVideoData();
                        if (App.MainViewModel.NeedDownedTip)
                        {
                            App.ShowToast(currentDownVideo.name + "  下载完成");
                        }
                        //进行下一个下载
                        isDownding = false;
                        BeginDownVideos();
                    });
                    Debug.WriteLine("下载完成");
                }
                else
                {
                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                    {
                        currentDownVideo.IsLoadError = true;
                        currentDownVideo.IsLoading = false;
                        isDownding = false;
                        //重新下载
                        BeginDownVideos();
                        //if (request != null)
                        //{
                        //    request.AllowReadStreamBuffering = false;
                        //    request.BeginGetResponse(new AsyncCallback(GetVideoData), request);
                        //}
                    });
                }
                streamToWriteTo.Close();
               
            }
            catch (Exception e)
            {
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    currentDownVideo.IsLoadError = true;
                    currentDownVideo.IsLoading = false;
                    isDownding = false;
                    //重新下载
                    BeginDownVideos();
                });
            }
         
        }
     
        public void SaveVideoData() 
        {

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
                if (videoInfo.downloadUrl[i].name == App.DownVideoModel.CurrentDefinitionName)
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
        private string currentDefinitionName;
        public string CurrentDefinitionName
        {
            get 
            {
                if (WpStorage.GetIsoSetting("CurrentDefinitionName") != null)
                {
                    currentDefinitionName = WpStorage.GetIsoSetting("CurrentDefinitionName").ToString();
                }
                else
                {
                    currentDefinitionName = "高清";
                }
                return currentDefinitionName; 
            }
            set
            {
                if (currentDefinitionName != value)
                {
                    currentDefinitionName = value;
                    WpStorage.SetIsoSetting("CurrentDefinitionName", currentDefinitionName);
                    NotifyPropertyChanged("CurrentDefinitionName");
                }
            }
        }
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
