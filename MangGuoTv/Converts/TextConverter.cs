using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MangGuoTv.Resources;

namespace MangGuoTv.Converts
{
   public class TextConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string retValue = value.ToString();
            //获取空格之后的字符
            int index = retValue.IndexOf(" ");
            if (index != -1) 
            {
               retValue = retValue.Substring(retValue.IndexOf(" "));
            }
           
            return retValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
    }
}
