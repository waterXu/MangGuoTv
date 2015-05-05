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
            string retValue = "";
            bool boolVlaue = (bool)value;
            //if (boolVlaue)
            //{
            //    if (parameter!=null && parameter.ToString().Equals("NickName"))
            //    {
            //        retValue = DbFMCommonData.NickName ?? (DbFMCommonData.NickName = AppResources.LoginFirst);
            //    }
            //    else
            //    {
            //        retValue = AppResources.ChangeAccount;
            //    }
            //}
            //else
            //{
            //    if (parameter != null && parameter.ToString().Equals("NickName"))
            //    {
            //        retValue = AppResources.LoginFirst;
            //    }
            //    else
            //    {
            //        retValue = AppResources.AccountLogin;
            //    }
            //}
            return retValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
    }
}
