using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.Models
{
    public class CommentResult 
    {
        /// <summary>
        /// 返回code 200为正确
        /// </summary>
        public string err_code { get; set; }
        public string err_msg { get; set; }
        public List<Comment> data { get; set; }
    }
    public class Comment
    {
        public string comment { get; set; }
        public string commentAvatar { get; set; }
        public string commentBy { get; set; }
        public string commentId { get; set; }
        public string date { get; set; }
        public string from { get; set; }
        public string up { get; set; }
    }
}
