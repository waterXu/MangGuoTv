using MangGuoTv.Models;
using MangGuoTv.PopUp;
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

namespace MangGuoTv.Views
{
    public class ChannelScrollView 
    {
        public ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        private ScrollViewer imageScroll { get; set; }
        private double scrollImageWidth = 250;
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
            scrollView.Content = stackPanel;
        }
        public ScrollViewer LoadChannelDetail(List<ChannelDetail> channelDetails)
        {
            foreach (ChannelDetail channeldetail in channelDetails)
            {
                switch (channeldetail.type)
                {
                    case "banner":
                        CreateBanner(channeldetail.templateData);
                        break;
                    case "normalAvatorText":
                        CreateNorLandscapeImages(channeldetail.templateData,180,4);
                        break;
                    case "largeLandScapeNodesc":
                    case "normalLandScapeNodesc":
                        CreateLandscapeImage(channeldetail.templateData);
                        break;
                    case "normalLandScape":
                        CreateNorLandscapeImages(channeldetail.templateData,210,2);
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
                stackPanel.Children.Add(CreateImageView(PopupManager.screenWidth - 20, template, 250));
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
                Grid myGrid = new Grid();
                myGrid.Height = 150;
                myGrid.Margin = new Thickness(10,5,5,10);
                Image liveImage = new Image();
                liveImage.Source = new BitmapImage(new Uri(template.picUrl,UriKind.RelativeOrAbsolute));
                liveImage.Height = 130;
                liveImage.Width = 150;
                liveImage.HorizontalAlignment = HorizontalAlignment.Left;
                TextBlock text = new TextBlock();
                text.Margin = new Thickness(liveImage.Width+10,0,0,0);
                text.VerticalAlignment = VerticalAlignment.Center;
                text.Text = template.name;
                myGrid.Children.Add(liveImage);
                myGrid.Children.Add(text);
                stackPanel.Children.Add(myGrid);
            }
          
        }
        private void CreateBanner(List<ChannelTemplate> list)
        {
            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(1000);
            totalScrollImages = list.Count;


            double imageWidth = PopupManager.screenWidth - 20;
            double imageHieght = 250;
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
            StackPanel imagesPanel = new StackPanel();
            imagesPanel.Orientation = Orientation.Horizontal;
            imageScroll.Content = imagesPanel;
            imagesPanel.Height = imageHieght;

            imagesGrid.Children.Add(imageScroll);
           
            foreach (ChannelTemplate template in list) 
            {
                Image image = new Image();
                image.Width = imageWidth;
                image.Height = imageHieght;
                image.Source =new BitmapImage(new Uri(template.picUrl,UriKind.RelativeOrAbsolute));
                image.Stretch = Stretch.UniformToFill;
                image.DataContext = template;
                image.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Image_Tap);
                imagesPanel.Children.Add(image);
            }
            stackPanel.Children.Add(imagesGrid);
            //timer.Tick += new EventHandler(Timer_Tick);
            //timer.Start();
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
        private string xaml = string.Empty;

        private Grid CreateImageView(double width, ChannelTemplate template, double height)
        {
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
            imageGrid.DataContext = template;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            return imageGrid;
        }
        private string rankXaml = string.Empty;
        private Grid CreateRankImageView(double width, ChannelTemplate template, double height) 
        {
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
            imageGrid.DataContext = template;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(GridImage_Tap);
            return imageGrid;
        }
        private void GridImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ChannelTemplate template = (sender as Grid).DataContext as ChannelTemplate;
            OperationImageTap(template);
        }
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ChannelTemplate template = (sender as Image).DataContext as ChannelTemplate;
            OperationImageTap(template);

        }
        private void OperationImageTap(ChannelTemplate template) 
        {
            switch (template.jumpType)
            {
                case "videoPlayer":
                    App.PlayerModel.VideoId = template.videoId;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.PlayerPageName, UriKind.Relative)); 
                    break;
                case "subjectPage":
                    MoreSubject.subjectId = template.subjectId;
                    MoreSubject.speicalName = template.name;
                    MoreSubject.isMoreChannel = false;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
                    break;
                case "webView":
                    break;
                case "livePlayer":
                    break;
                default:
                    break;
            }
        }
    }
}
