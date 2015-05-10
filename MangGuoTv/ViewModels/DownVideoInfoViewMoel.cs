using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class DownVideoInfoViewMoel:ViewModelBase
    {
        private string videoId;
        public string VideoId
        {
            get
            {
                return videoId;
            }
            set
            {
                if (value != videoId)
                {
                    videoId = value;
                    NotifyPropertyChanged("VideoId");
                }
            }
        }
        private string videoIndex;
        public string VideoIndex
        {
            get
            {
                return videoIndex;
            }
            set
            {
                if (value != videoIndex)
                {
                    videoIndex = value;
                    NotifyPropertyChanged("VideoIndex");
                }
            }
        }
        public string DownUrl { get; set; }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        private string image;
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (value != image)
                {
                    image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }
        private string localImage;
        public string LocalImage
        {
            get
            {
                return localImage;
            }
            set
            {
                if (value != localImage)
                {
                    localImage = value;
                    NotifyPropertyChanged("LocalImage");
                }
            }
        }
        private string desc;
        public string Desc
        {
            get
            {
                return desc;
            }
            set
            {
                if (value != desc)
                {
                    desc = value;
                    NotifyPropertyChanged("Desc");
                }
            }
        }
        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }
        private string localDownloadUrl;
        public string LocalDownloadUrl
        {
            get
            {
                return localDownloadUrl;
            }
            set
            {
                if (value != localDownloadUrl)
                {
                    localDownloadUrl = value;
                    NotifyPropertyChanged("LocalDownloadUrl");
                }
            }
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    NotifyPropertyChanged("IsLoading");
                }
            }
        }
        private bool isLoaded;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            set
            {
                if (value != isLoaded)
                {
                    isLoaded = value;
                    NotifyPropertyChanged("IsLoaded");
                }
            }
        }
        private double loadProgress;
        public double LoadProgress
        {
            get
            {
                return loadProgress;
            }
            set
            {
                if (value != loadProgress)
                {
                    loadProgress = value;
                    NotifyPropertyChanged("LoadProgress");
                }
            }
        }
    }
}
