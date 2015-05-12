using MangGuoTv.Commands;
using MangGuoTv.Models;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MangGuoTv.ViewModels
{
    public class MainViewModel:ViewModelBase
    {

        public MainViewModel()
        {
            this.AllChannels = new List<ChannelInfo>();
            this.LastVideoRemember = new ObservableCollection<DownVideoInfoViewMoel>();
            this.RememberVideos = new ObservableCollection<DownVideoInfoViewMoel>();
            loadRememberVideoData();
        }
       
        private ObservableCollection<DownVideoInfoViewMoel> lastVideoRemember;
        public ObservableCollection<DownVideoInfoViewMoel> LastVideoRemember
        {
            get { return lastVideoRemember; }
            set
            {
                lastVideoRemember = value;
                NotifyPropertyChanged("LastVideoRemember");
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> rememberVideos;
        public ObservableCollection<DownVideoInfoViewMoel> RememberVideos
        {
            get { return rememberVideos; }
            set
            {
                rememberVideos = value;
                NotifyPropertyChanged("RememberVideos");
            }
        }
        private string lastVideosRememberIso { get { return "lastVideosRemember.dat"; } }
        private string VideosRememberIso {get {return "VideosRemember.dat";}}
        private string videoSavePath { get { return "DownVideos\\"; } }
        public void loadRememberVideoData()
        {
            //加载最近观看视频数据
            string lastVideos = WpStorage.ReadIsolatedStorageFile(lastVideosRememberIso);
            if (!string.IsNullOrEmpty(lastVideos))
            {
                LastVideoRemember = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(lastVideos);
            }
            //加载所有视频记录
            string allVideos = WpStorage.ReadIsolatedStorageFile(VideosRememberIso);
            if (!string.IsNullOrEmpty(allVideos))
            {
                RememberVideos = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(allVideos);
            }
        }
        public void AddRememberVideo(VideoInfo videoInfo)
        {
            DownVideoInfoViewMoel video = new DownVideoInfoViewMoel();
            video.Name = videoInfo.name;
            video.Image = videoInfo.image;
            video.VideoId = videoInfo.videoId;
            HttpHelper.httpGet(video.Image, (imageAr) =>
            {
                byte[] imgdata = HttpHelper.SyncResultToByte(imageAr);
                if (imgdata != null)
                {
                    string imageType = video.Image.Remove(0, video.Image.Length - 4);
                    WpStorage.SaveFilesToIsoStore(videoSavePath + video.VideoId.ToString() + imageType, imgdata);
                    video.LocalImage = videoSavePath + video.VideoId.ToString() + imageType;
                }
                if (RememberVideos.Count > 20) 
                {
                    RememberVideos.RemoveAt(0);
                }
                RememberVideos.Add(video);
                if (LastVideoRemember.Count > 1)
                {
                    LastVideoRemember.RemoveAt(0);
                }
                LastVideoRemember.Add(video);
                SaveVideoData();
            });
        }
        public void SaveVideoData()
        {
            string lastVideoidData = null;
            if (LastVideoRemember.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                lastVideoidData = JsonConvert.SerializeObject(LastVideoRemember);
            }
            WpStorage.SaveStringToIsoStore(lastVideosRememberIso, lastVideoidData);
            string allReVideoidData = null;
            if (RememberVideos.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                allReVideoidData = JsonConvert.SerializeObject(RememberVideos);
            }
            WpStorage.SaveStringToIsoStore(VideosRememberIso, allReVideoidData);
        }
        public void LoadChannels() 
        {
            string strFileContent = string.Empty;
            if (!CommonData.ChannelLoaded)
            {
                if (WpStorage.isoFile.FileExists(CommonData.ChannelStorage))
                {
                    strFileContent = WpStorage.ReadIsolatedStorageFile(CommonData.ChannelStorage);
                }
                else
                {
                    using (Stream stream = Application.GetResourceStream(new Uri("channels.txt", UriKind.Relative)).Stream)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            strFileContent = reader.ReadToEnd();
                        }
                    }
                }

                AllChannelsData channels = JsonConvert.DeserializeObject<AllChannelsData>(strFileContent);
                CommonData.LockedChannel = channels.lockedChannel;
                CommonData.NormalChannel = channels.normalChannel;
            }
            this.AllChannels.AddRange(CommonData.LockedChannel);
            this.AllChannels.AddRange(CommonData.NormalChannel);
        }
        public List<ChannelInfo> AllChannels { get; set; }

        #region Command

        private DelegateCommand<string> _appSettingCommand;
        public DelegateCommand<string> AppSettingCommand
        {
            get
            {
                return _appSettingCommand ?? (_appSettingCommand = new DelegateCommand<string>((settingId) =>
                {
                    switch (settingId) 
                    {
                        //查看缓存
                        case "1":
                            CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.DownVideoPage, UriKind.RelativeOrAbsolute));
                            break;
                        //设置
                        case "2":
                            break;
                        //打分
                        case "3":
                             Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=56605010-6c29-4268-b7c2-2f97c2280579"));
                            break;
                        //意见反馈
                        case "4":
                             string strForamt = string.Format(" Device Info：\r\n {0} \r\n OS Version: \r\n {1} \r\n App Version:{2} \r\n NetWorkInfo:\r\n {3} \r\n "
                            , DeviceUtil.GetDeviceName() + DeviceUtil.GetManufactor(), DeviceUtil.GetOSVersion(), DeviceUtil.GetAppVersion(), DeviceUtil.GetNetWorkType());

                            EmailComposeTask emailTask = new EmailComposeTask()
                            {
                                To = "xuyanlan@outlook.com",
                                Subject = "芒果TV意见反馈",
                                Body = strForamt,
                            };
                            try
                            {
                                emailTask.Show();
                            }
                            catch (Exception e)
                            {

                            }
                            break;
                        //推荐应用
                        case "5":
                            //Windows.System.Launcher.LaunchUriAsync(new Uri("zune:navigate?appid=56605010-6c29-4268-b7c2-2f97c2280579"));
                            MarketplaceDetailTask marketplaceTask = new MarketplaceDetailTask();

                            marketplaceTask.ContentIdentifier = "56605010-6c29-4268-b7c2-2f97c2280579";

                            try
                            {
                                marketplaceTask.Show();
                            }
                            catch
                            {

                            }
                            break;
                        default:
                            break;
                    }
                }));
            }
        }
        #endregion
    }
}
