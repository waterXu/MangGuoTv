using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MangGuoTv.Converts
{
    public class BitmapConverter : IValueConverter
    {
       public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
           BitmapImage videoImage = new BitmapImage();
           string LocalImage = value.ToString();
           if(string.IsNullOrEmpty(LocalImage))return null;
            if (WpStorage.isoFile.FileExists(LocalImage))
            {
                using (IsolatedStorageFileStream isoFileStream = new IsolatedStorageFileStream(LocalImage, FileMode.Open, FileAccess.ReadWrite, WpStorage.isoFile))
                {
                    videoImage.SetSource(isoFileStream);
                }
            }
            //else
            //{
            //    videoImage = new BitmapImage(new Uri(parameter.ToString(), UriKind.RelativeOrAbsolute));
            //}
            return videoImage;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
            
    }
}
