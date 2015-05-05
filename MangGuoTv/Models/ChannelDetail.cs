using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MangGuoTv.Models
{
    public class channelDetailResult 
    {
        /// <summary>
        /// 返回code 200为正确
        /// </summary>
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public List<ChannelDetail> data { get; set; }

    }
    /// <summary>
    /// 频道详情
    /// </summary>
    public class ChannelDetail
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 类型 转换成枚举
        /// </summary>
        //public CommonData.ShowType Type { get { return (CommonData.ShowType)Enum.Parse(typeof(CommonData.ShowType), type); } }
        /// <summary>
        /// 频道模板数据
        /// </summary>
        public List<ChannelTemplate> templateData { get; set; }
    }
    /// <summary>
    /// 频道模板
    /// </summary>
    public class ChannelTemplate 
    {
        public string name { get; set; }
        /// <summary>
        /// 跳转类型
        /// </summary>
        public string jumpType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subjectId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 播放地址
        /// </summary>
        public string playUrl { get; set; }
        public string icon { get; set; }
        public string tag { get; set; }
        public string desc { get; set; }
        /// <summary>
        /// 视频id
        /// </summary>
        public string videoId { get; set; }
        public string hotDegree { get; set; }
        public string hotType { get; set; }
        public string playTimeIconUrl { get; set; }
        public string webUrl { get; set; }
        public string splitItem { get; set; }
        public string canShare { get; set; }
        public string ext { get; set; }
    }
}
