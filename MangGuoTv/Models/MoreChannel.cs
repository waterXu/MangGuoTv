using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
    public class MoreChannelResult
    {
        /// <summary>
        /// 返回code 200为正确
        /// </summary>
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public List<MoreChannel> data { get; set; }

    }
    /// <summary>
    /// 频道模板
    /// </summary>
    public class MoreChannel
    {
        public string name { get; set; }
        public string image { get; set; }
        public string tag { get; set; }
        public string desc { get; set; }
        public string icon { get; set; }
        public string publishTime { get; set; }
        public string videoId { get; set; }
        public string playCount { get; set; }
    }
}
