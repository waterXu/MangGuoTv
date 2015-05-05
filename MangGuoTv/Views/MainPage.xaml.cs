﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.Resources;
using System.Reflection;
using MangGuoTv.Models;
using Newtonsoft.Json;
using System.IO;
using MangGuoTv.Views;

namespace MangGuoTv
{
    public partial class MainPage : PhoneApplicationPage
    {
      
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            //BuildLocalizedApplicationBar();
            string strFileContent = string.Empty;
            if(!CommonData.ChannelLoaded)
            {
                using (Stream stream = Application.GetResourceStream(new Uri("channels.txt", UriKind.Relative)).Stream)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        strFileContent = reader.ReadToEnd();
                    }
                }
                AllChannelsData channels = JsonConvert.DeserializeObject<AllChannelsData>(strFileContent);
                CommonData.LockedChannel = channels.lockedChannel;
                CommonData.NormalChannel = channels.normalChannel;
            }
            for (int i = 0; i < CommonData.NormalChannel.Count / 2; i++)
            {
                PivotItemControl pivot = new PivotItemControl(CommonData.NormalChannel[i]);
                MainPivot.Items.Add(pivot.pivotItem);
            }
        }
       
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            CallbackManager.Mainpage = this;
            HttpHelper.LoadChannelList();
        //    string strFileContent = string.Empty;

        //    using (Stream stream = Application.GetResourceStream(new Uri("channels.txt", UriKind.Relative)).Stream)
        //    {
        //        using (StreamReader reader = new StreamReader(stream))
        //        {
        //            strFileContent = reader.ReadToEnd();
        //        }
        //    }
        //    AllChannelsData channels = JsonConvert.DeserializeObject<AllChannelsData>(strFileContent);
        //    CommonData.LockedChannel = channels.lockedChannel;
        //    CommonData.NormalChannel = channels.normalChannel;
        //    for (int i = 0; i < CommonData.NormalChannel.Count / 2; i++)
        //    {
        //        PivotItemControl pivot = new PivotItemControl(CommonData.NormalChannel[i]);
        //        MainPivot.Items.Add(pivot.pivotItem);
        //    }
        }
        


       
        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        internal void DataContextLoaded(bool isSuccess)
        {
            //if (!isSuccess)
            //{
            //    string strFileContent = string.Empty;

            //    using (Stream stream = Application.GetResourceStream(new Uri("channels.txt", UriKind.Relative)).Stream)
            //    {
            //        using (StreamReader reader = new StreamReader(stream))
            //        {
            //            strFileContent = reader.ReadToEnd();
            //        }
            //    }
            //    AllChannelsData channels = JsonConvert.DeserializeObject<AllChannelsData>(strFileContent);
            //    CommonData.LockedChannel = channels.lockedChannel;
            //    CommonData.NormalChannel = channels.normalChannel;
            //}
            //for (int i = 0; i < CommonData.NormalChannel.Count / 2; i++)
            //{
            //    PivotItemControl pivot = new PivotItemControl(CommonData.NormalChannel[i]);
            //    MainPivot.Items.Add(pivot.pivotItem);
            //}
           // string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + CommonData.LockedChannel[0].channelId + "&type=" + CommonData.LockedChannel[0].type;
           // HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
           // System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
        }

        private void OmnibusPivotItem_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                try
                {
                    List<ChannelDetail> channelDetails = JsonConvert.DeserializeObject<List<ChannelDetail>>(result);
                    this.Dispatcher.BeginInvoke(() =>
                    {
                        LoadChannelDetail(channelDetails);
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误");
                }
            }
            else
            {
            }
        }

        private void LoadChannelDetail(List<ChannelDetail> channelDetails)
        {
            
        }
    }
}