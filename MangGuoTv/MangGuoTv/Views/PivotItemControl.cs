﻿using MangGuoTv.Models;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Windows.Storage;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;

namespace MangGuoTv.Views
{
    public class PivotItemControl 
    {
        private ChannelInfo channel { get; set; }
        public PivotItem pivotItem;
        public ChannelScrollView scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        private bool LoadedComplete = false;
        private List<ChannelDetail> AllChannelData = new List<ChannelDetail>();
        public PivotItemControl( ChannelInfo channelInfo) 
        {
            channel = channelInfo;
            pivotItem = new PivotItem();
            pivotItem.Margin = new Thickness(0, 10, 0, 0);
            pivotItem.Loaded += new System.Windows.RoutedEventHandler(PivotItem_Loaded);
            TextBlock textBlock = new TextBlock();
            textBlock.Text = channelInfo.channelName;
            textBlock.FontSize = 35;
            textBlock.Width = (channelInfo.channelName.Length == 2) ? 80 : 110;
            pivotItem.Header = textBlock;
            scrollView = new ChannelScrollView();
            scrollView.channel = channel;
            pivotItem.Content = scrollView.scrollView;
        }
        private void PivotItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
            if (LoadedComplete) return;
            App.ShowLoading();
            string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + channel.channelId + "&type=" + channel.type;
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
        }
        private void LoadChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                channelDetailResult channelDetails = null;
                try
                {
                     channelDetails = JsonConvert.DeserializeObject<channelDetailResult>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                }
                if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode) 
                {
                    CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                    {
                        AllChannelData = channelDetails.data;
                        scrollView.LoadChannelDetail(AllChannelData);
                        scrollView.HideReload();
                        App.HideLoading();
                        LoadedComplete = true;
                    });
                }
            }
            else
            {
                //if (CommonData.NetworkStatus != "None") 
                //{
                //    App.ShowToast("获取数据失败，请检查网络或重试");
                //}
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    scrollView.loadGrid.Tap -= new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadData);
                    scrollView.loadGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadData);
                    App.HideLoading();
                    scrollView.ShowReload();
                });
               
            }
        }

        private void ReloadData(object sender, System.Windows.Input.GestureEventArgs e)
        {
            scrollView.HideReload();
            App.ShowLoading();
            string channelInfoUrl = CommonData.GetChannelInfoUrl + "&channelId=" + channel.channelId + "&type=" + channel.type;
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);

        }
    }
}
