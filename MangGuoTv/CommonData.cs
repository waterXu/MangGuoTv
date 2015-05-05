using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using MangGuoTv.Models;

namespace MangGuoTv
{
    public static class CommonData
    {
        #region Common  readonly Data
        /// <summary>
        /// 芒果tv  host url
        /// </summary>
        public static string MangGuoHost { get { return "http://mobile.api.hunantv.com/"; } }

        public static string CommonUrl { get { return "UserId="+UserId+"userId=&osVersion=4.4&device=sdk&appVersion=4.3.4&ticket=&channel=360dev&mac=i000000000000000&osType=android"; } }
        /// <summary>
        /// 视频列表url
        /// </summary>
        public static string VideoListUrl { get { return MangGuoHost + "channel/getList?"+CommonUrl; } }
        /// <summary>
        /// 频道详情
        /// </summary>
        public static string GetChannelInfoUrl { get { return MangGuoHost + "channel/getDetail?" + CommonUrl; } }
      
        #endregion

        #region IsolatedStorage FileName Or KeyName
        /// <summary>
        /// 是否第一次使用该App
        /// </summary>
        public static string IsFirstUse { get { return "IsFirstUse"; } }
        /// <summary>
        /// 上次成功登陆用户名
        /// </summary>
        public static string UserName { get { return "UserName"; } }
        /// <summary>
        /// 上次成功登录密码
        /// </summary>
        public static string Password { get { return "Password"; } }
        /// <summary>
        /// 应用显示 模式 
        /// </summary>
        public static string ShowMode { get { return "ShowMode"; } }
        /// <summary>
        /// 主题图片路径
        /// </summary>
        public static string ThemePath { get { return "ThemePath"; } }
        /// <summary>
        /// 是否自定义主题
        /// </summary>
        public static string IsCustom { get { return "IsCustom"; } }
        /// <summary>
        /// 获取自定义主题路径
        /// </summary>
        public static string CustomJpgPath { get { return "Custom.jpg"; } }
        /// <summary>
        /// 获取用户头像路径
        /// </summary>
        public static string UserJpgPath { get { return "User.jpg"; } }
        /// <summary>
        /// 独立存储收藏hz列表名称
        /// </summary>
        public static string CollectName { get { return "CollectChannels"; } }
        /// <summary>
        /// 独立存储歌曲id列表名称
        /// </summary>
        public static string DownSongIdsName { get { return "DownSongIds"; } }
        /// <summary>
        /// 独立存储下载歌曲信息保存文件名
        /// </summary>
        public static string SongsSavePath { get { return "DownSongsInfo.dat"; } }
        /// <summary>
        /// 红心赫兹 id
        /// </summary>
        public static string HotChannelId { get { return "-3"; } }

        #endregion 

        #region get set

        public static string UserId { get; set; }
        /// <summary>
        /// 启屏时间
        /// </summary>
        public static int TimerCount = 2;
        /// <summary>
        /// 保存登录成功Token
        /// </summary>
        public static string Token { set; get;}
        /// <summary>
        /// 保存登录成功Expire
        /// </summary>
        public static string Expire { set; get; }
        /// <summary>
        /// 豆瓣昵称
        /// </summary>
        public static string NickName { set; get; }
        /// <summary>
        /// userId
        /// </summary>
        public static string UserID { set; get; }
        /// <summary>
        /// 豆瓣绑定邮箱
        /// </summary>
        public static string Email { get; set; }
        /// <summary>
        /// 当前网络状态
        /// </summary>
        public static string NetworkStatus { get; set; }
        /// <summary>
        /// 是否自动下载添加的红心歌曲
        /// </summary>
        public static bool AutoDownLoveSongInWifi { get; set; }
        /// <summary>
        /// 验证码 图片地址
        /// </summary>
        public static string CaptchaImgUrl { get; set; }
        /// <summary>
        /// 验证码id
        /// </summary>
        public static string CaptchaId { get; set; }

        #endregion
        #region other parame
        /// <summary>
        ///视频 图片显示类型
        /// </summary>
        //public enum ShowType 
        //{
        //    /// <summary>
        //    /// 轮播
        //    /// </summary>
        //    banner,
        //    /// <summary>
        //    /// 详情
        //    /// </summary>
        //    title,
        //    /// <summary>
        //    /// 一张横盘图片
        //    /// </summary>
        //    normalAvatorText,
        //    normalLandScape,
        //    rankList,
        //    largeLandScapeNodesc,
        //    unknowModType1,
        //    unknowModType2
        //}
        public enum CallbackType
        {
            Login = 1,
            LoadedData = 2,
            LoadVideoBack = 3,
            DownVideoBack = 4,
            OperationBack = 5
        }


        /// <summary>
        /// 定义popup回调函数
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public delegate void InformCallback(int action, bool isSuccess,string type =null);
        /// <summary>
        /// 回调函数
        /// </summary>
        public static InformCallback informCallback { get; set; }

        #endregion


        public static bool MainPageLoaded { get; set; }
        public static List<ChannelInfo> LockedChannel { get; set; }
        public static List<ChannelInfo> NormalChannel { get; set; }

        public static bool ChannelLoaded { get; set; }
    }
}
