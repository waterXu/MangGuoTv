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
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using MangGuoTv.Popups;

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
            get 
            {
                if (lastVideoRemember == null)
                {
                    lastVideoRemember = new ObservableCollection<DownVideoInfoViewMoel>();
                }
                return lastVideoRemember; 
            }
            set
            {
                lastVideoRemember = value;
                NotifyPropertyChanged("LastVideoRemember");
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> rememberVideos;
        public ObservableCollection<DownVideoInfoViewMoel> RememberVideos
        {
            get 
            {
                if (rememberVideos == null)
                {
                    rememberVideos = new ObservableCollection<DownVideoInfoViewMoel>();
                }
                return rememberVideos; 
            }
            set
            {
                rememberVideos = value;
                NotifyPropertyChanged("RememberVideos");
            }
        }
        private string lastVideosRememberIso { get { return CommonData.rememberVideoSavePath +"lastVideosRemember.dat"; } }
        private string VideosRememberIso { get { return CommonData.rememberVideoSavePath + "VideosRemember.dat"; } }
        //private string rememberVideoSavePath { get { return "RememberVideos\\"; } }
        public void loadRememberVideoData()
        {
            //加载最近观看视频数据
            string lastVideos = WpStorage.ReadIsolatedStorageFile(lastVideosRememberIso);
            if (!string.IsNullOrEmpty(lastVideos))
            {
                LastVideoRemember = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(lastVideos);
            }
            else
            {
                LastVideoRemember = null;
            }
            //加载所有视频记录
            string allVideos = WpStorage.ReadIsolatedStorageFile(VideosRememberIso);
            if (!string.IsNullOrEmpty(allVideos))
            {
                RememberVideos = JsonConvert.DeserializeObject<ObservableCollection<DownVideoInfoViewMoel>>(allVideos);
            }
            else
            {
                RememberVideos = null;
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
                    WpStorage.SaveFilesToIsoStore(CommonData.rememberVideoSavePath + video.VideoId.ToString() + imageType, imgdata);
                    video.LocalImage =CommonData.rememberVideoSavePath + video.VideoId.ToString() + imageType;
                }
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    //检查记录是否相同 相同则删除相同项
                    for (int i = 0; i < RememberVideos.Count; i++)
                    {
                        if (RememberVideos[i].VideoId == video.VideoId)
                        {
                            RememberVideos.RemoveAt(i);
                            break;
                        }
                    }
                    if (RememberVideos.Count > 20)
                    {
                        RememberVideos.RemoveAt(0);
                    }
                    RememberVideos.Add(video);
                    //检查最近记录是否相同 相同则删除相同项
                    for (int i = 0; i < LastVideoRemember.Count; i++)
                    {
                        if (LastVideoRemember[i].VideoId == video.VideoId)
                        {
                            LastVideoRemember.RemoveAt(i);
                            break;
                        }
                    }
                    if (LastVideoRemember.Count > 1)
                    {
                        LastVideoRemember.RemoveAt(0);
                    }
                    LastVideoRemember.Add(video);
                    SaveRememberVideoData();
                });
            });
        }
        public void SaveRememberVideoData()
        {
            string lastVideoidData = null;
            if (LastVideoRemember != null && LastVideoRemember.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                lastVideoidData = JsonConvert.SerializeObject(LastVideoRemember);
            }
            WpStorage.SaveStringToIsoStore(lastVideosRememberIso, lastVideoidData);
            string allReVideoidData = null;
            if (RememberVideos != null && RememberVideos.Count > 0)
            {
                //把hashset表反序列化为字符串 存入独立存储
                allReVideoidData = JsonConvert.SerializeObject(RememberVideos);
            }
            WpStorage.SaveStringToIsoStore(VideosRememberIso, allReVideoidData);
            App.DownVideoModel.UpdateIsoSize();

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
                            CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SettingPage, UriKind.RelativeOrAbsolute));
                            break;
                        //打分
                        case "3":
                        //    try
                        //    {
                        //        Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=30c60bf9-e4c2-412e-8cc0-b53d15618f63"));

                        //        //Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=c700c407-1d91-4007-8474-b24271e25661"));
                        //    }
                        //    catch
                        //    {

                        //    }
                        //    break;
                        ////意见反馈
                        case "4":
                            // string strForamt = string.Format(" Device Info：\r\n {0} \r\n OS Version: \r\n {1} \r\n App Version:{2} \r\n NetWorkInfo:\r\n {3} \r\n "
                            //, DeviceUtil.GetDeviceName() + DeviceUtil.GetManufactor(), DeviceUtil.GetOSVersion(), DeviceUtil.GetAppVersion(), DeviceUtil.GetNetWorkType());

                            //EmailComposeTask emailTask = new EmailComposeTask()
                            //{
                            //    To = "xuyanlan@outlook.com",
                            //    Subject = "芒果TV意见反馈",
                            //    Body = strForamt,
                            //};
                            //try
                            //{
                            //    emailTask.Show();
                            //}
                            //catch (Exception e)
                            //{

                            //}
                            //break;
                            PopupManager.ShowUserControl(PopupManager.UserControlType.CommentControl);
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
                        case "6":
                            CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.AboutPage,UriKind.RelativeOrAbsolute));
                            break;
                        default:
                            break;
                    }
                }));
            }
        }
              //使用自定义主题
        private DelegateCommand _cleanAllIsoCommand;
        public DelegateCommand CleanAllIsoCommand
        {
            get
            {
                return _cleanAllIsoCommand ?? (_cleanAllIsoCommand = new DelegateCommand(() =>
                {
                    if (MessageBox.Show("确定要清除所有数据？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WpStorage.DeleteDirectory(CommonData.IsoRootPath);
                        IsolatedStorageSettings.ApplicationSettings.Clear();
                        NeedDownedTip = false;
                        App.DownVideoModel.StopGetVideoData();
                        App.DownVideoModel.loadLocalVideoData();
                        loadRememberVideoData();
                    }
                }));
            }
        }
        #endregion
         private bool _needDownedTip;
        /// <summary>
        /// 
        /// </summary>
         public bool NeedDownedTip

        {
            get 
            {
                if (WpStorage.GetIsoSetting("NeedDownedTip") != null)
                {
                    _needDownedTip = (bool)WpStorage.GetIsoSetting("NeedDownedTip");
                }
                else
                {
                    _needDownedTip = false;
                }
                return _needDownedTip; 
            }
            set
            {
                if (_needDownedTip != value)
                {
                    _needDownedTip = value;
                    WpStorage.SetIsoSetting("NeedDownedTip", _needDownedTip);
                    NotifyPropertyChanged("NeedDownedTip");
                }
            }
        }
       
    }
}
