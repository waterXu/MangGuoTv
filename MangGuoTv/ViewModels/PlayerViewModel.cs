using MangGuoTv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private List<VideoInfo> _AllDramas;
        public List<VideoInfo> AllDramas
        {
            get { return _AllDramas; }
            set
            {
                _AllDramas = value;
                NotifyPropertyChanged("AllDramas");
            }
        }
        private string videoIndex;
        public string VideoIndex
        {
            get { return videoIndex; }
            set
            {
                videoIndex = value;
            }
        }
    }
}
