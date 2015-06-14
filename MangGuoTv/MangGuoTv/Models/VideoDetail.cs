using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
    public class VideoDetailResult 
    {
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public VideoData data { get; set; }
    }
    public class VideoData 
    {
        public string reviewState { get; set; }
        public string displayType { get; set; }
        public List<VideoDefinition> videoSources { get; set; }
        public string years { get; set; }
        public List<VideoDefinition> downloadUrl { get; set; }
        public string adParams { get; set; }
        public VideoDetail detail {get;set;}
    }
    public class VideoDetail
    {
        public string pcUrl{ get;set;}
        public string videoId{ get;set;}
        public string director{ get;set;}
        public string player{ get;set;}
        public string typeName { get; set; }
        public string typeId { get; set; }
        public string image{ get;set;}
        public string desc{ get;set;}
        public string publishTime{ get;set;}
        public string name{ get;set;}
        public string playCount{ get;set;}
        public string time{ get;set;}
        public string favorite{ get;set;}
        public string isFull{ get;set;}
    }
}
