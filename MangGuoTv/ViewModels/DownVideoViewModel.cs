using MangGuoTv.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class DownVideoViewModel:ViewModelBase
    {
        public DownVideoViewModel()
        {
            //todo  加载本地的数据
            this.DowningVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            this.DownedVideo = new ObservableCollection<DownVideoInfoViewMoel>();
            loadLocalVideoData();
        }
        public void loadLocalVideoData()
        {
        }
        public void SaveVideoData() 
        {

        }
        public void BeginDownVideos() 
        {
        }
        public void AddDownVideo(VideoInfo videoInfo) 
        {
            DownVideoInfoViewMoel video = new DownVideoInfoViewMoel();
            video.Name = videoInfo.name;
            video.Image = videoInfo.image;
            video.Time = videoInfo.time;
            video.VideoId = videoInfo.videoId;
            video.VideoIndex = videoInfo.videoIndex;
            video.Desc = videoInfo.desc;
            // todo 
            video.DownUrl = videoInfo.downloadUrl[0].url;
            DowningVideo.Add(video);
        }
        #region Property
        private HashSet<string> downingVideoids;
        public HashSet<string> DowningVideoids 
        {
            get { return downingVideoids; }
            set
            {
                downingVideoids = value;
            }
        }
        private HashSet<string> downedVideoids;
        public HashSet<string> DownedVideoids
        {
            get { return downedVideoids; }
            set
            {
                downedVideoids = value;
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> downingVideo;
        public ObservableCollection<DownVideoInfoViewMoel> DowningVideo 
        {
            get { return downingVideo; }
            set
            { 
                downingVideo = value;
                NotifyPropertyChanged("DowningVideo");
            }
        }
        private ObservableCollection<DownVideoInfoViewMoel> downedVideo { get; set; }
        public ObservableCollection<DownVideoInfoViewMoel> DownedVideo
        {
            get { return downedVideo; }
            set
            {
                downedVideo = value;
                NotifyPropertyChanged("DownedVideo");
            }
        }
        #endregion
    }
}
