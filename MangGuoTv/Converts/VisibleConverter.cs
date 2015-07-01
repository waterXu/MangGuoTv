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
            if (value is Boolean)
            {
                bool val = (bool)value;
                return val ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            else 
            {
                string val = value.ToString();
                if (parameter != null) 
                {
                    if (!string.IsNullOrEmpty(val))
                    {
                        return val == "subjectPage" ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    }
                }
                else 
                {
                    return string.IsNullOrEmpty(val) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                }
             
            }
            return System.Windows.Visibility.Collapsed;
           
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
    }
}
