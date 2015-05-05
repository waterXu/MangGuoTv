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
    public partial class MoreSubject : PhoneApplicationPage
    {
        public static List<ChannelDetail> channelDetails;
      //  mobile.api.hunantv.com/channel/special?userId=&osVersion=4.4&device=sdk&appVersion=4.3.4&ticket=&channel=360dev&mac=i000000000000000&osType=android&subjectId=1052
        public static string subjectUrl { get; set; }
        public MoreSubject()
        {
            InitializeComponent();
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
            string channelInfoUrl = "mobile.api.hunantv.com/channel/special?userId=&osVersion=4.4&device=sdk&appVersion=4.3.4&ticket=&channel=360dev&mac=i000000000000000&osType=android&subjectId=1052";
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
        }
        private void LoadChannelCompleted(IAsyncResult ar)
        {
            string result = HttpHelper.SyncResultTostring(ar);
            if (result != null)
            {
                try
                {

                    channelDetailResult channelDetails = JsonConvert.DeserializeObject<channelDetailResult>(result);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ChannelScrollView scrollView = new ChannelScrollView();
                        MainGrid.Children.Add(scrollView.scrollView);
                        scrollView.LoadChannelDetail(channelDetails.data);
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误");
                }
            }
            else
            {
                App.ShowToast("获取数据失败，请检查网络或重试");
            }
        }
    }
}