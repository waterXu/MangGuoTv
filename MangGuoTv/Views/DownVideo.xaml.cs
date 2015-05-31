using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangGuoTv.ViewModels;
using MangGuoTv.PopUp;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Info;

namespace MangGuoTv.Views
{
    public partial class DownVideo : PhoneApplicationPage
    {
        public DownVideo()
        {
            InitializeComponent();
            LoadEditIcon();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.DownVideoModel;
            //App.DownVideoModel.BeginDownVideos();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            this.DataContext = null;
        }
        private int pvoitIndex = 0;
        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DowningVideos.ItemContainerStyle = null;
            DowningVideos.SelectionMode = SelectionMode.Single;
            DowningVideos.SelectedIndex = -1;
            DownVideoList.ItemContainerStyle = null;
            DownVideoList.SelectionMode = SelectionMode.Single;
            DownVideoList.SelectedIndex = -1;
            IsEditMode = false;
            pvoitIndex = MainPivot.SelectedIndex;
            if (pvoitIndex == 2) 
            {
                for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
                {
                    this.ApplicationBar.Buttons.RemoveAt(i);
                }
            }
            else 
            {
                LoadEditIcon();
            }
        }

        private void DowningVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DowningVideos.SelectedIndex == -1)
            {
                if (IsEditMode) deleteSelectBtn.IsEnabled = false;
                return;
            }
            if (IsEditMode)
            {
                if (DowningVideos.SelectedItems.Count == 0)
                {
                    deleteSelectBtn.IsEnabled = false;
                }
                else if (DowningVideos.SelectedItems.Count > 0)
                {
                    deleteSelectBtn.IsEnabled = true;
                }
                return;
            }
        }
        private void DownedVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择模式不跳转
            if (DownVideoList.SelectedIndex == -1)
            {
                if (IsEditMode) deleteSelectBtn.IsEnabled = false;
                return;
            }
            if (IsEditMode)
            {
                if (DownVideoList.SelectedItems.Count == 0)
                {
                    deleteSelectBtn.IsEnabled = false;
                }
                else if (DownVideoList.SelectedItems.Count > 0)
                {
                    deleteSelectBtn.IsEnabled = true;
                }
                return;
            }
            DownVideoInfoViewMoel DownVideo = DownVideoList.SelectedItem as DownVideoInfoViewMoel;
            if (DownVideo == null) return;
            App.PlayerModel.VideoId = DownVideo.VideoId;
            App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.LoaclType;
            this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
        }

        #region applicationBar method
        private ApplicationBarIconButton deleteSelectBtn { get; set; }
        private ApplicationBarIconButton deleteAllBtn { get; set; }
        private ApplicationBarIconButton closeBtn { get; set; }
        public bool IsEditMode { get; set; }

        private void DeleteSelectIcon_Click(object sender, EventArgs e)
        {
            if (pvoitIndex == 0) 
            {
                if (MessageBox.Show("确定要删除选中正在缓存的剧集吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    List<DownVideoInfoViewMoel> videos = new List<DownVideoInfoViewMoel>();
                    foreach (DownVideoInfoViewMoel Video in DowningVideos.SelectedItems)
                    {
                        videos.Add(Video);
                    }
                    foreach (DownVideoInfoViewMoel Video in videos)
                    {
                        if (App.DownVideoModel.currentDownVideo != null && App.DownVideoModel.currentDownVideo.VideoId == Video.VideoId) 
                        {
                            App.DownVideoModel.StopGetVideoData();
                        }
                        bool isdelete = App.DownVideoModel.DowningVideo.Remove(Video);
                        App.DownVideoModel.DowningVideoids.Remove(Video.VideoId);
                        if (Video.LocalImage != null)
                        {
                            if (WpStorage.isoFile.FileExists(Video.LocalImage))
                            {
                                WpStorage.isoFile.DeleteFile(Video.LocalImage);
                            }
                        }
                        if (WpStorage.isoFile.FileExists(CommonData.videoSavePath + Video.VideoId.ToString() + ".mp4"))
                        {
                            WpStorage.isoFile.DeleteFile(CommonData.videoSavePath + Video.VideoId.ToString() + ".mp4");
                        }
                        App.DownVideoModel.SaveVideoData();
                    }
                    LoadEditIcon();
                }
            }
            else if (pvoitIndex == 1) 
            {
                if (MessageBox.Show("确定要删除选中已经缓存的剧集吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    List<DownVideoInfoViewMoel> videos = new List<DownVideoInfoViewMoel>();
                    foreach (DownVideoInfoViewMoel Video in DownVideoList.SelectedItems) 
                    {
                        videos.Add(Video);
                    }
                    foreach (DownVideoInfoViewMoel Video in videos)
                    {
                        App.DownVideoModel.DownedVideo.Remove(Video);
                        App.DownVideoModel.DownedVideoids.Remove(Video.VideoId);
                        DownVideoList.SelectedItems.Remove(Video);
                        if (Video.LocalImage != null)
                        {
                            if (WpStorage.isoFile.FileExists(Video.LocalImage))
                            {
                                WpStorage.isoFile.DeleteFile(Video.LocalImage);
                            }
                        }
                        if (Video.LocalDownloadUrl != null)
                        {
                            if (WpStorage.isoFile.FileExists(Video.LocalDownloadUrl))
                            {
                                WpStorage.isoFile.DeleteFile(Video.LocalDownloadUrl);
                            }
                        }
                        App.DownVideoModel.SaveVideoData();
                    }
                    LoadEditIcon();
#if DEBUG
                    long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
                    long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
                    long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
                    System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
                }
            }
          
        }

        private void DeleteAllIcon_Click(object sender, EventArgs e)
        {
            if (pvoitIndex == 0)
            {
                if (MessageBox.Show("确定要删除所有正在缓存的剧集吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    App.DownVideoModel.StopGetVideoData();
                    foreach (DownVideoInfoViewMoel Video in App.DownVideoModel.DowningVideo)
                    {
                        if (Video.LocalImage != null)
                        {
                            if (WpStorage.isoFile.FileExists(Video.LocalImage))
                            {
                                WpStorage.isoFile.DeleteFile(Video.LocalImage);
                            }
                        }
                        if (WpStorage.isoFile.FileExists(CommonData.videoSavePath + Video.VideoId.ToString() + ".mp4"))
                        {
                            WpStorage.isoFile.DeleteFile(CommonData.videoSavePath + Video.VideoId.ToString() + ".mp4");
                        }
                    }
                    App.DownVideoModel.DowningVideo = null;
                    App.DownVideoModel.DowningVideoids = null;
                    App.DownVideoModel.SaveVideoData();
                    LoadEditIcon();
                }
            }
            else if (pvoitIndex == 1)
            {
                if (MessageBox.Show("确定要删除所有已经缓存的剧集吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    App.DownVideoModel.DownedVideo = null;
                    App.DownVideoModel.DownedVideoids = null;
                    WpStorage.DeleteDirectory(CommonData.videoSavePath);
                    App.DownVideoModel.SaveVideoData();
                    LoadEditIcon();
#if DEBUG
                    long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
                    long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
                    long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
                    System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
                }
            }
          
        }
        private void CloseIcon_Click(object sender, EventArgs e)
        {
            LoadEditIcon();
            DowningVideos.ItemContainerStyle = null;
            DowningVideos.SelectionMode = SelectionMode.Single;
            DownVideoList.ItemContainerStyle = null;
            DownVideoList.SelectionMode = SelectionMode.Single;
            IsEditMode = false;
        }


        private void LoadEditIcon()
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string editIcon = "/Images/Icons/edit.png";
            ApplicationBarIconButton editBtn = new ApplicationBarIconButton(new Uri(editIcon, UriKind.Relative));
            editBtn.Text = "编辑";
            editBtn.Click += new EventHandler(Edit_Click);
            this.ApplicationBar.Buttons.Add(editBtn);

            DowningVideos.ItemContainerStyle = null;
            DowningVideos.SelectionMode = SelectionMode.Single;
            DownVideoList.ItemContainerStyle = null;
            DownVideoList.SelectionMode = SelectionMode.Single;
            IsEditMode = false;
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            string closeIcon = "/Images/Icons/cancel.png";
            closeBtn = new ApplicationBarIconButton(new Uri(closeIcon, UriKind.Relative));
            closeBtn.Text = "取消";
            closeBtn.Click += new EventHandler(CloseIcon_Click);
            this.ApplicationBar.Buttons.Add(closeBtn);

            string deleteAllIcon = "/Images/Icons/deleteall.png";
            deleteAllBtn = new ApplicationBarIconButton(new Uri(deleteAllIcon, UriKind.Relative));
            deleteAllBtn.Text = "全部删除";
            deleteAllBtn.Click += new EventHandler(DeleteAllIcon_Click);
            this.ApplicationBar.Buttons.Add(deleteAllBtn);

            string deleteSelectIcon = "/Images/Icons/delete.png";
            deleteSelectBtn = new ApplicationBarIconButton(new Uri(deleteSelectIcon, UriKind.Relative));
            deleteSelectBtn.Text = "删除选中";
            deleteSelectBtn.IsEnabled = false;
            deleteSelectBtn.Click += new EventHandler(DeleteSelectIcon_Click);
            this.ApplicationBar.Buttons.Add(deleteSelectBtn);

            Style itemStyle = Resources["ListBoxItemStyle1"] as Style;
            if (pvoitIndex == 0)
            {
                DowningVideos.ItemContainerStyle = itemStyle;
                DowningVideos.SelectionMode = SelectionMode.Multiple;
            }
            else if (pvoitIndex == 1) 
            {
                DownVideoList.ItemContainerStyle = itemStyle;
                DownVideoList.SelectionMode = SelectionMode.Multiple;
            }
          
            IsEditMode = true;
        }

        #endregion

        private void DefinitionListClick(object sender, RoutedEventArgs e)
        {
            DefinitionList.Visibility = System.Windows.Visibility.Visible;
        }

        private void DefinitionTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock definitionText = sender as TextBlock;
            App.DownVideoModel.CurrentDefinitionName = definitionText.Text;
            DefinitionList.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}