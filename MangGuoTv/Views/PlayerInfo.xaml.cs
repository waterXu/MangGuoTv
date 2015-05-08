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
    public partial class PlayerInfo : PhoneApplicationPage
    {
        public PlayerInfo()
        {
            InitializeComponent();
            this.DataContext = App.PlayerModel;
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

        private void PivotItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DetailItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DramaItem_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("获取剧集列表 url：" + CommonData.GetVideoListUrl + "&videoId=" + App.PlayerModel.VideoIndex);
            HttpHelper.httpGet(CommonData.GetVideoListUrl + "&videoId=" + App.PlayerModel.VideoIndex, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    VideoInfoResult videosResult = null;
                    try
                    {
                        videosResult = JsonConvert.DeserializeObject<VideoInfoResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                    }
                    if (videosResult != null && videosResult.err_code == HttpHelper.rightCode)
                    {
                        CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                        {
                            App.PlayerModel.AllDramas = videosResult.data;
                        });
                    }
                }
                else
                {
                    App.ShowToast("获取数据失败，请检查网络或重试");
                }
            });
        }

        private void CommentItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AllDramas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}