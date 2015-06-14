using MangGuoTv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Markup;
using Windows.Storage.Pickers;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Info;
using MangGuoTv.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Data;
using MangGuoTv.Popups;

namespace MangGuoTv.Views
{
    public class ChannelScrollView :ViewModelBase
    {
        public ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        private ScrollViewer imageScroll { get; set; }
        private double scrollImageWidth = 250;
        public ChannelInfo channel { get; set; }
        /// <summary>
        ///图片滚动计时器
        /// </summary>
        private DispatcherTimer timer;
        public ChannelScrollView() 
        {
            scrollView = new ScrollViewer();
            //scrollView.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            stackPanel = new StackPanel();
            CreateReload();
            stackPanel.Children.Add(loadGrid);
            scrollView.Content = stackPanel;
            
        }
        public ScrollViewer LoadChannelDetail(List<ChannelDetail> channelDetails)
        {
            int maxCount = 20;
            if (channelDetails.Count < 20)
            {
                maxCount = channelDetails.Count;
            }
            for (int i = 0; i < maxCount;i++ )
            {
                ChannelDetail channeldetail = channelDetails[i];
                switch (channeldetail.type)
                {
                    case "banner":
                        //CreateBanner(channeldetail.templateData);
                        CreateLandscapeImage(channeldetail.templateData);
                        break;
                    case "normalAvatorText":
                        CreateNorLandscapeImages(channeldetail.templateData, 180, 4);
                        break;
                    case "largeLandScapeNodesc":
                    case "largeLandScape":
                    case "normalLandScapeNodesc":
                    case "aceSeason":
                        CreateLandscapeImage(channeldetail.templateData);
                        break;
                    case "normalLandScape":
                    case "roundAvatorText":
                    case "tvPortrait":
                        CreateNorLandscapeImages(channeldetail.templateData, 180, 2);
                        break;
                    case "title":
                        CreateTitleView(channeldetail.templateData);
                        break;
                    case "rankList":
                        CreateRankImages(channeldetail.templateData);
                        break;
                    case "live":
                        CreateLiveView(channeldetail.templateData);
                        break;
                    case "unknowModType1":
                        //CreateNorLandscapeImages(channeldetail.templateData);
                        break;
                    case "unknowModType2":
                        //CreateNorLandscapeImages(channeldetail.templateData);
                        break;
                    default:
                        break;
                }
            }
            return scrollView;
        }

       
        private void CreateTitleView(List<ChannelTemplate> list)
        {
            foreach (ChannelTemplate template in list)
            {
                Grid titlePanel = new Grid();
                titlePanel.Margin = new Thickness(20, 10, 20, 10);
                titlePanel.Height = 40;
                Canvas myRectangle = new Canvas();
                myRectangle.Background = new SolidColorBrush(Color.FromArgb(255, 238, 98, 33));
                myRectangle.Width = 10;
                myRectangle.Height = 40;
                myRectangle.VerticalAlignment = VerticalAlignment.Center;
                myRectangle.HorizontalAlignment = HorizontalAlignment.Left;
                titlePanel.Children.Add(myRectangle);
                TextBlock titleBlock = new TextBlock();
                titleBlock.VerticalAlignment = VerticalAlignment.Center;
                titleBlock.HorizontalAlignment = HorizontalAlignment.Left;
                titleBlock.Text = template.name;
                titleBlock.Height = 50;
                titleBlock.Margin = new Thickness(myRectangle.Width+15, 0, 0, 0);
                titleBlock.FontSize = 25;
                titlePanel.Children.Add(titleBlock);
                if (template.jumpType == "subjectPage")
                {
                    TextBlock moreBlock = new TextBlock();
                    moreBlock.DataContext = template;
                    moreBlock.Text = "更多>>";
                    moreBlock.FontSize = 25;
                    moreBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 98, 33));
                    moreBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    moreBlock.VerticalAlignment = VerticalAlignment.Center;
                    moreBlock.Height = 50; 
                    moreBlock.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(MoreChannelSubject);
                   
                    titlePanel.Children.Add(moreBlock);
                }
                stackPanel.Children.Add(titlePanel);
            }
        }

        private void MoreChannelSubject(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ChannelTemplate template = (sender as TextBlock).DataContext as ChannelTemplate;
            MoreSubject.subjectId = template.subjectId;
            MoreSubject.speicalName = template.name;
            MoreSubject.isMoreChannel = false;
            CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
        }
        /// <summary>
        /// 创建多个ImagesView的Grid
        /// </summary>
        /// <param name="list">数据列表List<ChannelTemplate></param>
        /// <param name="gridImageHeight">每个imageView的高度</param>
        /// <param name="lineCount">每行显示多少个imageView</param>
        private void CreateNorLandscapeImages(List<ChannelTemplate> list, double gridImageHeight, int lineCount)
        {
            Grid myGrid = new Grid();
            myGrid.Height = gridImageHeight * Math.Ceiling((double)list.Count/ lineCount);
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.ShowGridLines = false;
            // Define the Columns
            for (int i = 0; i < lineCount; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef);
            }
            for (int i = 0; i < Math.Ceiling((double)list.Count/ lineCount); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef);
            }


            for (int i = 0; i < list.Count; i++)
            {
                ChannelTemplate template = list[i];
                double imageWidth = (double)(PopupManager.screenWidth - 20 - 5*lineCount) / lineCount;
                Grid imageGrid = CreateImageView(imageWidth, template, gridImageHeight - 10);
                imageGrid.Margin = new Thickness(5, 5, 5, 5);
                Grid.SetColumn(imageGrid, i % lineCount);
                Grid.SetRow(imageGrid, i / lineCount);
                myGrid.Children.Add(imageGrid);
            }
            stackPanel.Children.Add(myGrid);
        }

        private void CreateLandscapeImage(List<ChannelTemplate> list)
        {
            foreach (ChannelTemplate template in list)
            {
                stackPanel.Children.Add(CreateImageView(PopupManager.screenWidth - 20, template, 220));
            }
        }
        private void CreateRankImages(List<ChannelTemplate> list)
        {
            foreach (ChannelTemplate template in list)
            {
                stackPanel.Children.Add(CreateRankImageView(PopupManager.screenWidth - 20, template, 150));
            }
        }
        private void CreateLiveView(List<ChannelTemplate> list)
        {
            foreach(ChannelTemplate template in list)
            {
                VideoViewModel videoData = new VideoViewModel
                {
                    width = 150,
                    hight = 130,
                    name = template.name,
                    jumpType = template.jumpType,
                    subjectId = template.subjectId,
                    picUrl = template.picUrl,
                    playUrl = template.playUrl,
                    tag = template.tag,
                    desc = template.desc,
                    videoId = template.videoId,
                    hotDegree = template.hotDegree,
                    webUrl = template.webUrl,
                    rank = template.rank
                };
                Grid myGrid = new Grid();
                myGrid.Height = 150;
                myGrid.Margin = new Thickness(10,5,5,10);
                Image liveImage = new Image();
                liveImage.Source = new BitmapImage(new Uri(videoData.picUrl, UriKind.RelativeOrAbsolute));
                liveImage.Height = 130;
                liveImage.Width = 150;
                liveImage.HorizontalAlignment = HorizontalAlignment.Left;
                TextBlock text = new TextBlock();
                text.Margin = new Thickness(liveImage.Width+10,0,0,0);
                text.VerticalAlignment = VerticalAlignment.Center;
                text.Text = videoData.name;
                myGrid.Children.Add(liveImage);
                myGrid.Children.Add(text);
                myGrid.DataContext = videoData;
                myGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
                stackPanel.Children.Add(myGrid);
            }
          
        }
#if DEBUG
        private void CreateBanner(List<ChannelTemplate> list)
        {
            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(1000);
            totalScrollImages = list.Count;


            double imageWidth = PopupManager.screenWidth - 20;
            double imageHieght = 200;
            Grid imagesGrid = new Grid();
            imagesGrid.Margin = new Thickness(0,0,0,10);
            imagesGrid.Width = imageWidth;
            imagesGrid.Height = imageHieght;
          
            imageScroll = new ScrollViewer();
            imageScroll.Height = imageHieght;
            imageScroll.Width = imageWidth;
            imageScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            imageScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //imageScroll.ManipulationStarted += new EventHandler<System.Windows.Input.ManipulationStartedEventArgs>(ImageScroll_StartMove);
            //imageScroll.ManipulationCompleted += new EventHandler<System.Windows.Input.ManipulationCompletedEventArgs>(ImageScroll_EndMove);
            //imageScroll.ManipulationDelta += new EventHandler<System.Windows.Input.ManipulationDeltaEventArgs>(ImageScroll_EndDelta);
            //imageScroll.SizeChanged += new SizeChangedEventHandler();
            //imageScroll.MouseEnter += new System.Windows.Input.MouseEventHandler(ImageScroll_StartMove);
            //imageScroll.MouseMove += new System.Windows.Input.MouseEventHandler(ImageScroll_Moveing);
            //imageScroll.MouseLeave += new System.Windows.Input.MouseEventHandler(ImageScroll_EndMove);
            StackPanel imagesPanel = new StackPanel();
            imagesPanel.Orientation = Orientation.Horizontal;
            imageScroll.Content = imagesPanel;
            imagesPanel.Height = imageHieght;

            imagesGrid.Children.Add(imageScroll);
           
            foreach (ChannelTemplate template in list) 
            {
                VideoViewModel videoData = new VideoViewModel
                {
                    name = template.name,
                    jumpType = template.jumpType,
                    subjectId = template.subjectId,
                    picUrl = template.picUrl,
                    playUrl = template.playUrl,
                    tag = template.tag,
                    desc = template.desc,
                    videoId = template.videoId,
                    hotDegree = template.hotDegree,
                    webUrl = template.webUrl,
                    rank = template.rank
                };
                Grid imageGrid = new Grid();
                imageGrid.Width = imageWidth;
                imageGrid.Height = imageHieght;
                Image image = new Image();
                image.Width = imageWidth;
                image.Height = imageHieght;
                image.Source = new BitmapImage(new Uri(videoData.picUrl, UriKind.RelativeOrAbsolute));
                image.Stretch = Stretch.UniformToFill;
              
                imageGrid.Children.Add(image);
                Border textCanvas = new Border();
                textCanvas.Height = 40;
                textCanvas.Background = new SolidColorBrush(Color.FromArgb(255, 106, 98, 33));
                textCanvas.VerticalAlignment = VerticalAlignment.Bottom;
                textCanvas.HorizontalAlignment = HorizontalAlignment.Left;
                TextBlock textNmae = new TextBlock();
                textNmae.FontSize = 20;
                textNmae.TextWrapping = TextWrapping.Wrap;
                textNmae.Text = videoData.name;
                textNmae.HorizontalAlignment = HorizontalAlignment.Left;
                textNmae.VerticalAlignment = VerticalAlignment.Center;
                textCanvas.Child = textNmae;
                imageGrid.Children.Add(textCanvas);
                imagesPanel.Children.Add(imageGrid);

                imageGrid.DataContext = videoData;
                imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            }
            stackPanel.Children.Add(imagesGrid);
            //timer.Tick += new EventHandler(Timer_Tick);
            //timer.Start();
        }
        private void ImageScroll_StartMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }
        private void ImageScroll_Moveing(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }
        private void ImageScroll_EndMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double horOffset = imageScroll.HorizontalOffset;
            int index = (int)Math.Round(horOffset / scrollImageWidth);
            imageScroll.ScrollToHorizontalOffset(index * scrollImageWidth);
        }
        int totalScrollImages = 0;
        int currentIndex = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            currentIndex++;
            if (currentIndex > totalScrollImages) 
            {
                currentIndex = 0;
            }
            imageScroll.ScrollToHorizontalOffset(currentIndex * scrollImageWidth);
        }

        private void ImageScroll_EndDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            double horOffset = imageScroll.HorizontalOffset;
            int index = (int)Math.Round(horOffset / scrollImageWidth);
            imageScroll.ScrollToHorizontalOffset(index * scrollImageWidth);
        }

        private void ImageScroll_EndMove(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            timer.Start();
        }

        private void ImageScroll_StartMove(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            timer.Stop();
        }
#endif
        private string xaml = string.Empty;
        List<BitmapImage> allBitmap = new List<BitmapImage>();
        private Grid CreateImageView(double width, ChannelTemplate template, double height)
        {
            VideoViewModel videoData = new VideoViewModel
            {
                width = (int)width,
                hight = (int)height,
                name = template.name,
                jumpType = template.jumpType,
                subjectId = template.subjectId,
                picUrl = template.picUrl,
                playUrl = template.playUrl,
                tag = template.tag,
                desc = template.desc,
                videoId = template.videoId,
                hotDegree = template.hotDegree,
                webUrl = template.webUrl,
                rank = template.rank
            };
            if (string.IsNullOrEmpty(xaml))
            {
                using (Stream stream = Application.GetResourceStream(new Uri("/MangGuoTv;component/Views/ImageView.xaml", UriKind.Relative)).Stream)
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
            imageGrid.DataContext = videoData;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            return imageGrid;
        }
        private string rankXaml = string.Empty;
        private Grid CreateRankImageView(double width, ChannelTemplate template, double height) 
        {
            VideoViewModel videoData = new VideoViewModel
            {
                //width = (int)width,
                //hight = (int)height,
                name = template.name,
                jumpType = template.jumpType,
                subjectId = template.subjectId,
                picUrl = template.picUrl,
                playUrl = template.playUrl,
                tag = template.tag,
                desc = template.desc,
                videoId = template.videoId,
                hotDegree = template.hotDegree,
                webUrl = template.webUrl,
                rank = template.rank
            };
            if (string.IsNullOrEmpty(rankXaml))
            {
                using (Stream stream = Application.GetResourceStream(new Uri("/MangGuoTv;component/Views/RankImageView.xaml", UriKind.Relative)).Stream)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        rankXaml = reader.ReadToEnd();
                    }
                }
            }
            Grid imageGrid = (Grid)XamlReader.Load(rankXaml);
            imageGrid.Width = width;
            imageGrid.Height = height;
            imageGrid.DataContext = videoData;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            return imageGrid;
        }
        private void GridImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VideoViewModel template = (sender as Grid).DataContext as VideoViewModel;
            OperationImageTap(template);
        }
        private void OperationImageTap(VideoViewModel template) 
        {
#if DEBUG
            long memory = DeviceStatus.ApplicationCurrentMemoryUsage / (1024 * 1024);
            long memoryLimit = DeviceStatus.ApplicationMemoryUsageLimit / (1024 * 1024);
            long memoryMax = DeviceStatus.ApplicationPeakMemoryUsage / (1024 * 1024);
            System.Diagnostics.Debug.WriteLine("当前内存使用情况：" + memory.ToString() + " MB 当前最大内存使用情况： " + memoryMax.ToString() + "MB  当前可分配最大内存： " + memoryLimit.ToString() + "  MB");
#endif
            
            switch (template.jumpType)
            {
                case "videoPlayer":
                    App.PlayerModel.VideoId = template.videoId;
                    App.PlayerModel.currentType = ViewModels.PlayerViewModel.PlayType.VideoType;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
                    break;
                case "subjectPage":
                    MoreSubject.subjectId = template.subjectId;
                    MoreSubject.speicalName = template.name;
                    MoreSubject.isMoreChannel = false;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
                    break;
                case "videoLibrary":
                    if (channel != null)
                    {
                        MoreChannelInfo.typeId = channel.libId;
                        MoreChannelInfo.name = channel.channelName;
                        CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.MoreChannelPageName, UriKind.Relative));
                    }
                    break;
                case "webView":
                    WebBrowserTask task = new WebBrowserTask();
                    task.Uri = new Uri(template.webUrl, UriKind.Absolute);
                    try
                    {
                        task.Show();
                    }
                    catch (Exception e)
                    {

                    }
                    break;
                case "livePlayer":
                    LivePlayer.liveUrl = template.playUrl;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.LivePlayerPage, UriKind.Relative));
                    break;
                case "concertLivePlayer":
                     LivePlayer.liveUrl = template.playUrl;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.LivePlayerPage, UriKind.Relative));
                    //App.ShowToast("抱歉，暂时不支持直播功能");
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("该播放类型暂时未实现"+template.jumpType);
                    App.ShowToast("该播放类型暂时未实现" + template.jumpType );
                    break;
            }
        }

        private void ClearImg()
        {
            for (int i = 0; i < allBitmap.Count; i++)
            {
                allBitmap[i] = new BitmapImage();
            }
        }


        public Grid loadGrid = null;
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
            }
        }

        public void ShowReload() 
        {
            loadGrid.Visibility = Visibility.Visible;
        }
        public void HideReload()
        {
            loadGrid.Visibility = Visibility.Collapsed;
        }
    }
}
