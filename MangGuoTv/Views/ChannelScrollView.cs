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

namespace MangGuoTv.Views
{
    public class ChannelScrollView 
    {
        public ScrollViewer scrollView { set; get; }
        private StackPanel stackPanel { get; set; }
        public ChannelScrollView() 
        {
            scrollView = new ScrollViewer();
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
                        break;
                    case "largeLandScapeNodesc":
                        CreateLandscapeImage(channeldetail.templateData);
                        break;
                    case "normalLandScape":
                        CreateNorLandscapeImages(channeldetail.templateData);
                        break;
                    case "title":
                        CreateTitleView(channeldetail.templateData);
                        break;
                    case "rankList":
                        break;
                    case "unknowModType1":
                        CreateNorLandscapeImages(channeldetail.templateData);
                        break;
                    case "unknowModType2":
                        CreateNorLandscapeImages(channeldetail.templateData);
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
                StackPanel titlePanel = new StackPanel();
                titlePanel.Margin = new Thickness(20, 0, 0, 0);
                titlePanel.Height = 40;
                titlePanel.Orientation = Orientation.Horizontal;
                Rectangle myRectangle = new Rectangle();
                myRectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 252, 135, 92));
                myRectangle.Width = 10;
                myRectangle.Height = 40;
                myRectangle.VerticalAlignment = VerticalAlignment.Center;
                titlePanel.Children.Add(myRectangle);
                TextBlock titleBlock = new TextBlock();
                titleBlock.VerticalAlignment = VerticalAlignment.Center;
                titleBlock.Text = template.name;
                titleBlock.Height = 50;
                titleBlock.Margin = new Thickness(15, 0, 0, 0);
                titlePanel.Children.Add(titleBlock);
                if (template.jumpType == "subjectPage")
                {
                    TextBlock moreBlock = new TextBlock();
                    moreBlock.Text = "            更多>>";
                    moreBlock.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(MoreChannelSubject);
                    // moreBlock.Command =
                    moreBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    moreBlock.Height = 50;
                    titlePanel.Children.Add(moreBlock);
                }
                stackPanel.Children.Add(titlePanel);
            }
        }

        private void MoreChannelSubject(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        private void CreateNorLandscapeImages(List<ChannelTemplate> list)
        {
            // Create the Grid
            double gridImageHeight = 210;
            Grid myGrid = new Grid();
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.ShowGridLines = false;
            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef1);
            myGrid.ColumnDefinitions.Add(colDef2);
            myGrid.Height = gridImageHeight * Math.Ceiling((double)list.Count * 0.5);
            for (int i = 0; i < Math.Ceiling((double)list.Count * 0.5); i++)
            {
                RowDefinition rowDef = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef);
            }


            for (int i = 0; i < list.Count; i++)
            {
                ChannelTemplate template = list[i];
                double imageWidth = (PopupManager.screenWidth - 20 - 10) * 0.5;
                Grid imageGrid = CreateImageView(imageWidth, template, gridImageHeight - 10);
                imageGrid.Margin = new Thickness(5, 5, 5, 5);
                Grid.SetColumn(imageGrid, i % 2);
                Grid.SetRow(imageGrid, i / 2);
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
        }
    }
}
