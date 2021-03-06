﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.IO.IsolatedStorage;

namespace MangGuoTv.Views
{
    public partial class StartPage : PhoneApplicationPage
    {
        private DispatcherTimer BgImgTimer = new DispatcherTimer();
        private int time = 2;
        public StartPage()
        {
            InitializeComponent();

            versionText.Text = DeviceUtil.GetAppVersion();
            BgImgTimer.Interval = new TimeSpan(1000);
            BgImgTimer.Tick += new EventHandler(BgImgTimer_Tick);
            BgImgTimer.Start();
            checkVersion();
        }
        private void checkVersion()
        {
            bool isNeedClean = false;
            try
            {
                if (WpStorage.GetIsoSetting("Version") != null)
                {
                    string version = WpStorage.GetIsoSetting("Version").ToString();
                    if (version != DeviceUtil.GetAppVersion())
                    {
                        isNeedClean = true;
                        WpStorage.SetIsoSetting("Version", DeviceUtil.GetAppVersion());
                    }
                }
                else
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Count > 0 || WpStorage.isoFile.DirectoryExists(CommonData.IsoRootPath))
                    {
                        isNeedClean = true;
                    }
                    WpStorage.SetIsoSetting("Version", DeviceUtil.GetAppVersion());
                }
                if (isNeedClean)
                {
                    if (MessageBox.Show("检测到版本更新，为避免数据冲突建议清除本地缓存，是否清除所有缓存(包括缓存视频!包括缓存视频!包括缓存视频!)？不想已经缓存好的视频在版本升级中被自动清除，请点击“取消”按钮，否则会被清除，谢谢~", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WpStorage.DeleteDirectory(CommonData.IsoRootPath);
                        IsolatedStorageSettings.ApplicationSettings.Clear();
                        WpStorage.SetIsoSetting("Version", DeviceUtil.GetAppVersion());
                    }
                }
            }
            catch { }
            App.BeginApp();
        }
        private void BgImgTimer_Tick(object sender, EventArgs e)
        {
            
            if (BgImgTimer == null)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.NavigationService.Navigate(new Uri(CommonData.MianPage, UriKind.RelativeOrAbsolute));
                });
            }
            else
            {
                if (time != 0)
                {
                    time--;
                    return;
                }
                BgImgTimer.Stop();
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.NavigationService.Navigate(new Uri(CommonData.MianPage, UriKind.RelativeOrAbsolute));
                });
            }
        }
    }
}