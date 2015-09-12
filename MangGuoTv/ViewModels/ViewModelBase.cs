using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MangGuoTv.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (null != handler)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            catch { }
        }
    }
}
