using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MangGuoTv.Converts
{
    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (parameter is Boolean)
            {
                bool val = (bool)value;
                return val? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            else 
            {
                string val = value.ToString();
                return string.IsNullOrEmpty(val) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }
           
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
    }
}
