using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MangGuoTv.ViewModels
{
    public class DownVideoInfoViewMoel : ViewModelBase
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
        private string loadStatus;
         public string LoadStatus
        {
            get
            {
                return loadStatus;
            }
            set
            {
                loadStatus = value;
                NotifyPropertyChanged("LoadStatus");
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
                    if (isLoading)
                    {
                        LoadStatus = "下载中";
                    }
                    else
                    {
                        LoadStatus = "等待中";
                    }
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
        private bool isLoadError;
        public bool  IsLoadError
        {
            get
            {
                return isLoadError;
            }
            set
            {
                isLoadError = value;
                if (isLoadError)
                {
                    LoadStatus = "缓存失败";
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
        private double maxProgress;
        public double MaxProgress
        {
            get
            {
                return maxProgress;
            }
            set
            {
                maxProgress = value;
                NotifyPropertyChanged("MaxProgress");
            }
        }
     
        private string size;
        public string Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                NotifyPropertyChanged("Size");
            }
        }
        private string loadsize;
        public string Loadsize
        {
            get
            {
                return loadsize;
            }
            set
            {
                loadsize = value;
                NotifyPropertyChanged("Loadsize");
            }
        }
    }
}
