using MangGuoTv.Models;
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
using MangGuoTv.PopUp;

namespace MangGuoTv.Views
{
    public class PivotItemControl 
    {
        private ChannelInfo channel { get; set; }
        public PivotItem pivotItem;
        public ChannelScrollView scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
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
            pivotItem.Content = scrollView.scrollView;
        }
        private void PivotItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
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
                        scrollView.LoadChannelDetail(channelDetails.data);
                    });
                }
            }
            else
            {
                App.ShowToast("获取数据失败，请检查网络或重试");
            }
        }
    }
}
