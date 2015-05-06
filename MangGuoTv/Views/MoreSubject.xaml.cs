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
        public static string subjectId { get; set; }
        public static string speicalName { get; set; }
        public MoreSubject()
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
            this.subjectName.Text = speicalName;
            string channelInfoUrl = CommonData.GetSpecialUrl + "&subjectId=" + subjectId;
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