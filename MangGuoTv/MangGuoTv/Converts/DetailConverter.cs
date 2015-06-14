using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MangGuoTv.Converts
{
    public class DetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string name = null;
            string detail = value.ToString();
            string detailType = parameter.ToString();
            switch (detailType) 
            {
                case "name":
                    name = "剧集 ： ";
                    break;
                case "typeName":
                    name = "类型 ： ";
                    break;
                case "director":
                    name = "导演 ： ";
                    break;
                case "player":
                    name = "演员 ： ";
                    break;
                case "desc":
                    break;
                default:
                    break;
            }
            return name + detail;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value;
        }
    }
}
