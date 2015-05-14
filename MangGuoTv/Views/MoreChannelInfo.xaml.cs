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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Markup;
using MangGuoTv.PopUp;

namespace MangGuoTv.Views
{
    public partial class MoreChannelInfo : PhoneApplicationPage
    {
        private ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        public static string typeId { get; set; }
        public static string name { get; set; }
        public Grid addTipGrid = null;
        private int pageCount = 0;
        public MoreChannelInfo()
        {
            InitializeComponent();
            scrollView = new ScrollViewer();
            //scrollView.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            stackPanel = new StackPanel();
            CreateReload();
            stackPanel.Children.Add(loadGrid);
            scrollView.Content = stackPanel;
            MainGrid.Children.Add(scrollView);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            if (addTipGrid == null)
            {
                CreateAddGrid();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            stackPanel.Children.Remove(addTipGrid);
        }
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            pageCount = 0;
            this.channelName.Text = name;
            string channelInfoUrl = CommonData.GetMoreChannelInfo + "&typeId=" + typeId + "&pageCount=" + pageCount;
            App.ShowLoading();
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
        }
        private void LoadChannelCompleted(IAsyncResult ar)
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
                        App.HideLoading();
                        loadGrid.Visibility = Visibility.Collapsed;
                        if (pageCount > 0) 
                        {
                            stackPanel.Children.Remove(addTipGrid);
                        }
                        AddChannelView(channelDetails.data, 200, 2);
                        pageCount++;
                    });
                }
            }
            else
            {
                App.ShowToast("获取数据失败，请检查网络或重试");
                App.HideLoading();
                loadGrid.Visibility = Visibility.Visible;
            }
        }


        #region Create View Menthod
        private Grid loadGrid = null;

        private void CreateAddGrid()
        {
            addTipGrid = new Grid();
            addTipGrid.Background = new SolidColorBrush(Color.FromArgb(255, 238, 98, 33));
            addTipGrid.Height = 80;
            addTipGrid.Width = 300;
            TextBlock addText = new TextBlock();
            addText.Text = "点击加载更多";
            addText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            addText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            addTipGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(MoreChannelData_Tap);
            addTipGrid.Children.Add(addText);
        }

        private void MoreChannelData_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ReloadData(null,null);
        }
        internal void CreateReload()
        {
            if (loadGrid == null)
            {
                loadGrid = new Grid();
                loadGrid.Margin = new Thickness(0, 200, 0, 0);
                loadGrid.HorizontalAlignment = HorizontalAlignment.Center;
                loadGrid.VerticalAlignment = VerticalAlignment.Center;
                Image loadImage = new Image();
                loadImage.Width = 100;
                loadImage.Height = 100;
                loadImage.Source = new BitmapImage(new Uri("/Images/reload.png", UriKind.RelativeOrAbsolute));
                loadImage.VerticalAlignment = VerticalAlignment.Center;
                TextBlock loadText = new TextBlock();
                loadText.Text = "获取数据失败，点击重试";
                loadText.FontSize = 25;
                loadText.VerticalAlignment = VerticalAlignment.Center;
                loadGrid.Children.Add(loadImage);
                loadGrid.Children.Add(loadText);
                loadGrid.Visibility = Visibility.Collapsed;
                loadGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ReloadData);
            }
        }
        private void ReloadData(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ShowLoading();
            loadGrid.Visibility = Visibility.Collapsed;
            string channelInfoUrl = CommonData.GetMoreChannelInfo + "&typeId=" + typeId + "&pageCount=" + pageCount;
            HttpHelper.httpGet(channelInfoUrl, LoadChannelCompleted);
            System.Diagnostics.Debug.WriteLine("频道详情channelInfoUrl ：" + channelInfoUrl);

        }
        private void AddChannelView(List<MoreChannel> list, double gridImageHeight, int lineCount)
        {
            Grid myGrid = new Grid();
            myGrid.Height = gridImageHeight * Math.Ceiling((double)list.Count / lineCount);
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.ShowGridLines = false;
            // Define the Columns
            for (int i = 0; i < lineCount; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef);
            }
            for (int i = 0; i < Math.Ceiling((double)list.Count / lineCount); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef);
            }


            for (int i = 0; i < list.Count; i++)
            {
                MoreChannel template = list[i];
                double imageWidth = (double)(PopupManager.screenWidth - 20 - 5 * lineCount) / lineCount;
                Grid imageGrid = CreateImageView(imageWidth, template, gridImageHeight - 10);
                imageGrid.Margin = new Thickness(5, 5, 5, 5);
                Grid.SetColumn(imageGrid, i % lineCount);
                Grid.SetRow(imageGrid, i / lineCount);
                myGrid.Children.Add(imageGrid);
            }
            stackPanel.Children.Add(myGrid);
            stackPanel.Children.Add(addTipGrid);
        }
        private string xaml = string.Empty;
        private Grid CreateImageView(double width, MoreChannel template, double height)
        {
            if (string.IsNullOrEmpty(xaml))
            {
                using (Stream stream = Application.GetResourceStream(new Uri("/MangGuoTv;component/Views/MoreChannelImageView.xaml", UriKind.Relative)).Stream)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        xaml = reader.ReadToEnd();
                    }
                }
            }
            Grid imageGrid = (Grid)XamlReader.Load(xaml);
            imageGrid.Width = width;
            imageGrid.Height = height;
            imageGrid.DataContext = template;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            return imageGrid;
        }

        private void GridImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MoreChannel template = (sender as Grid).DataContext as MoreChannel;
            if (template == null) return;
            App.PlayerModel.VideoId = template.videoId;
            CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative));
        }
        #endregion
    }
}