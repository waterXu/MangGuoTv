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

namespace MangGuoTv.Views
{
    public partial class RememberVideos : PhoneApplicationPage
    {
        public RememberVideos()
        {
            InitializeComponent();
            LoadEditIcon();
        }

        private void RememberVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择模式不跳转
            if (RememberVideoList.SelectedIndex == -1)
            {
                if (IsEditMode) deleteSelectBtn.IsEnabled = false;
                return;
            }
            if (IsEditMode)
            {
                if (RememberVideoList.SelectedItems.Count == 0)
                {
                    deleteSelectBtn.IsEnabled = false;
                }
                else if (RememberVideoList.SelectedItems.Count > 0)
                {
                    deleteSelectBtn.IsEnabled = true;
                }
            }
            else
            {
                DownVideoInfoViewMoel DownVideo = RememberVideoList.SelectedItem as DownVideoInfoViewMoel;
                if (DownVideo == null) return;
                App.PlayerModel.VideoId = DownVideo.VideoId;
                this.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
            }
         
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.MainViewModel;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.DataContext = null;
            CallbackManager.currentPage = null;
        }

        #region applicationBar method
        private ApplicationBarIconButton deleteSelectBtn { get; set; }
        private ApplicationBarIconButton deleteAllBtn { get; set; }
        private ApplicationBarIconButton closeBtn { get; set; }
        public bool IsEditMode { get; set; }
    
        private void DeleteSelectIcon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除选中播放记录吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (DownVideoInfoViewMoel rememberVideo in RememberVideoList.SelectedItems) 
                {
                    App.MainViewModel.RememberVideos.Remove(rememberVideo);
                    //没有删除成功则代表不是最近播放记录
                    if (!App.MainViewModel.LastVideoRemember.Remove(rememberVideo)) 
                    {
                        if (WpStorage.isoFile.FileExists(rememberVideo.LocalImage))
                        {
                            WpStorage.isoFile.DeleteFile(rememberVideo.LocalImage);
                        }
                    }
                }
                App.MainViewModel.SaveRememberVideoData();
                RememberVideoList.ItemContainerStyle = null;
                RememberVideoList.SelectionMode = SelectionMode.Single;
                IsEditMode = false;
                LoadEditIcon();
            }
        }

        private void DeleteAllIcon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除所有播放记录吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (DownVideoInfoViewMoel rememberVideo in App.MainViewModel.RememberVideos)
                {
                    if (WpStorage.isoFile.FileExists(rememberVideo.LocalImage))
                    {
                        WpStorage.isoFile.DeleteFile(rememberVideo.LocalImage);
                    }
                }
                App.MainViewModel.RememberVideos = null;
                App.MainViewModel.LastVideoRemember = null;
                App.MainViewModel.SaveRememberVideoData();
                RememberVideoList.ItemContainerStyle = null;
                RememberVideoList.SelectionMode = SelectionMode.Single;
                IsEditMode = false;
                LoadEditIcon();
            }
        }
        private void CloseIcon_Click(object sender, EventArgs e)
        {
            LoadEditIcon();
            RememberVideoList.ItemContainerStyle = null;
            RememberVideoList.SelectionMode = SelectionMode.Single;
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
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            for (int i = this.ApplicationBar.Buttons.Count - 1; i >= 0; i--)
            {
                this.ApplicationBar.Buttons.RemoveAt(i);
            }
            RememberVideoList.SelectedIndex = -1;
            string closeIcon = "/Images/Icons/cancel.png";
            closeBtn = new ApplicationBarIconButton(new Uri(closeIcon, UriKind.Relative));
            closeBtn.Text = "取消";
            closeBtn.Click += new EventHandler(CloseIcon_Click);
            this.ApplicationBar.Buttons.Add(closeBtn);

            string deleteAllIcon = "/Images/Icons/delete.png";
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
            RememberVideoList.ItemContainerStyle = itemStyle;
            RememberVideoList.SelectionMode = SelectionMode.Multiple;
            IsEditMode = true;
        }
     
        #endregion


    }
}