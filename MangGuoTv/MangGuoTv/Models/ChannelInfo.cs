using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
   public class ChannelInfo
    {
        //channelId: 1001,
        //channelName: "精选",
        //iconUrl: "http://i3.hunantv.com/p/20141126/1529276385C.png",
        //hasMore: "record",
        //libId: 0,
        //type: "normal"
        /// <summary>
        /// 频道ID
        /// </summary>
        public string channelId { get; set; }
        /// <summary>
        /// 频道名称
        /// </summary>
        public string channelName { get; set; }
        /// <summary>
        /// 频道icon
        /// </summary>
        public string iconUrl { get; set; }
        /// <summary>
        /// 更多
        /// </summary>
        public string hasMore { get; set; }
        public string libId { get; set; }
        /// <summary>
        /// 频道播放类型  normal为正常  live为直播
        /// </summary>
        public string type { get; set; }
    }
    public class AllChannelsData 
    {
        public string versionCode { get; set; }
        public List<ChannelInfo> lockedChannel { get; set; }
        public List<ChannelInfo> normalChannel { get; set; }
    }
   public  class ChannelsResult 
    {
        /// <summary>
        /// 返回code 200为正确
        /// </summary>
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public AllChannelsData data { get; set; }
    }
}
