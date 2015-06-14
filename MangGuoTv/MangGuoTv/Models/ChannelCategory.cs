using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
    /// <summary>
    /// 频道扩展返回
    /// </summary>
    class ChannelCategoryResult 
    { 
        /// <summary>
        /// 返回code 200为正确
        /// </summary>
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public List<ChannelCategory> data { get; set; }
    }
    /// <summary>
    /// 频道类型扩展
    /// </summary>
   public class ChannelCategory
    {
        /// <summary>
        /// 频道类型id  对应ChannelInfo  libId
        /// </summary>
        public string typeId { get; set; }
        /// <summary>
        /// 频道类型名称
        /// </summary>
        public string typeName { get; set; }
        /// <summary>
        /// 所有过滤类型
        /// </summary>
        public List<FilterItem> filterItems { get; set; }
    }
   public class FilterItem
    {
        /// <summary>
        /// 过滤名称
        /// </summary>
        public string filtername { get; set; }
        /// <summary>
        /// 过滤类型
        /// </summary>
        public string filtertype { get; set; }
        /// <summary>
        /// 过滤列表
        /// </summary>
        public List<Filter> filters { get; set; }
    }
    public class Filter 
    {
        /// <summary>
        /// 过滤id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 过滤名称
        /// </summary>
        public string name { get; set; }
    }
}
