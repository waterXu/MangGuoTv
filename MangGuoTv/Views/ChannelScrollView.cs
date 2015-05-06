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

namespace MangGuoTv.Views
{
    public class ChannelScrollView 
    {
        public ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
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
                        CreateNorLandscapeImages(channeldetail.templateData,150,4);
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
                titlePanel.Margin = new Thickness(20, 0, 0, 0);
                titlePanel.Height = 40;
                Rectangle myRectangle = new Rectangle();
                myRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 238, 98, 33));
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

        private void CreateBanner(List<ChannelTemplate> list)
        {
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
            // 加载Rectangle
            Grid imageGrid = (Grid)XamlReader.Load(xaml);
            imageGrid.Width = width;
            imageGrid.Height = height;
            imageGrid.DataContext = template;
            imageGrid.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ImageGrid_Tap);
            return imageGrid;
        }

        private void ImageGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ChannelTemplate template = (sender as Grid).DataContext as ChannelTemplate;
            switch (template.jumpType) 
            {
                case "videoPlayer":
                    break;
                case "subjectPage":
                    MoreSubject.subjectId = template.subjectId;
                    MoreSubject.speicalName = template.name;
                    CallbackManager.currentPage.NavigationService.Navigate(new Uri(CommonData.SpecialPageName, UriKind.Relative));
                    break;
                case "webView":
                    break;
                default:
                    break;
            }
           
        }
    }
}
