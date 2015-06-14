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
using MangGuoTv.Popups;

namespace MangGuoTv.Views
{
    public partial class Search : PhoneApplicationPage
    {
        public Search()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHotSearch();
         
        }

        private void LoadHotSearch()
        {
            App.ShowLoading();
            HttpHelper.httpGet(CommonData.HotSearch, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    MoreChannelResult channelDetails = null;
                    try
                    {
                        channelDetails = JsonConvert.DeserializeObject<MoreChannelResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        App.HideLoading();
                    }
                    if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
                    {
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            HotSearch.ItemsSource = channelDetails.data;
                            App.HideLoading();
                        });
                    }
                }
                else
                {
                    App.HideLoading();
                }
            });
        }

        /// <summary>
        /// 当输入框获取焦点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            PopupManager.Input_GotFocus((Control)sender, this.LayoutRoot);
        }
        /// <summary>
        /// 当输入框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            PopupManager.Input_LostFocus(this.LayoutRoot);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string name = SearchName.Text;
            if (string.IsNullOrEmpty(name)) return;
            BeginSearch(name);
        }
        private void BeginSearch(string name)
        {
            System.Diagnostics.Debug.WriteLine("获取搜索结果 url：" + CommonData.SearchUrl + "&name=" + name);
            HttpHelper.httpGet(CommonData.SearchUrl + "&name=" + name, (ar) =>
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    MoreChannelResult channelDetails = null;
                    try
                    {
                        channelDetails = JsonConvert.DeserializeObject<MoreChannelResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误" + ex.Message);
                        App.HideLoading();
                    }
                    if (channelDetails != null && channelDetails.err_code == HttpHelper.rightCode)
                    {
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            if (channelDetails.data.Count > 0)
                            {
                                SerachInfo.ItemsSource = channelDetails.data;
                            }
                            else
                            {
                                App.ShowToast("没有搜索结果");
                            }
                        });
                    }
                }
                else
                {
                    App.HideLoading();
                }
            });
        }

        private void SerachInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoreChannel template = SerachInfo.SelectedItem as MoreChannel;
            if (template != null)
            {
                App.PlayerModel.VideoId = template.videoId;
                App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
                this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
            }
          
        }

        private void HotSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             MoreChannel template = HotSearch.SelectedItem as MoreChannel;
             if (template != null && !string.IsNullOrEmpty(template.name))
             {
                 BeginSearch(template.name);
             }
        }
    }
}