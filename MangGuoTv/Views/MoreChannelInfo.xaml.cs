using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.Models;
using Newtonsoft.Json;

namespace MangGuoTv.Views
{
    public partial class MoreChannelInfo : PhoneApplicationPage
    {
        public static string typeId { get; set; }
        public static string name { get; set; }
        private int pageCount = 0;
        public MoreChannelInfo()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
        }
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.channelName.Text = name;
            string channelInfoUrl = CommonData.GetMoreChannelInfo + "&type=" + typeId + "&pageCount=" + pageCount;
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
                    this.Dispatcher.BeginInvoke(() =>
                    {
                        ChannelScrollView scrollView = new ChannelScrollView();
                        MainGrid.Children.Add(scrollView.scrollView);
                        scrollView.LoadChannelDetail(channelDetails.data);
                    });
                }
            }
            else
            {
                App.ShowToast("获取数据失败，请检查网络或重试");
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}