using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
    public class JsonError 
    {
        public string err_code { get; set; }
        public string err_msg { get; set; }
    }
    public class VideoInfoResult
    {
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public ObservableCollection<VideoInfo> data { get; set; }
    }
    public class VideoInfo
    {
        public string videoId { get; set; }
        public string videoIndex { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string desc { get; set; }
        public string time { get; set; }
        public List<VideoDefinition> downloadUrl { get; set; }
    }
    public class VideoDefinition
    {
        public string definition { get; set; }
        public string name { get; set; }
        public string url { get; set; }

    }
}
