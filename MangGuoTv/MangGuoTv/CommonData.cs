﻿using System;
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

        public static string CommonUrl { get { return "userId="+UserId+"&osVersion=4.4&device=sdk&appVersion=4.3.4&ticket=&channel=360dev&mac=i000000000000000&osType=ios"; } }
        /// <summary>
        /// 视频列表url
        /// </summary>
        public static string VideoListUrl { get { return MangGuoHost + "channel/getList?"+CommonUrl; } }
        /// <summary>
        /// 频道详情
        /// </summary>
        public static string GetChannelInfoUrl { get { return MangGuoHost + "channel/getDetail?" + CommonUrl; } }
        /// <summary>
        /// 获取更多专题
        /// </summary>
        public static string GetSpecialUrl { get { return MangGuoHost + "channel/special?" + CommonUrl; } }
        /// <summary>
        ///获取节目列表
        /// </summary>
        public static string GetVideoListUrl { get { return MangGuoHost + "video/getList?" + CommonUrl + "&pageSize=30"; } }
        /// <summary>
        /// 获取视频详情
        /// </summary>
        public static string GetVideoDetailUrl { get { return MangGuoHost + "video/getById?" + CommonUrl; } }
        /// <summary>
        /// 获取视频数据
        /// </summary>
        public static string GetVideoResourceUrl { get { return MangGuoHost + "video/getSource?" + CommonUrl; } }
        /// <summary>
        /// 评论
        /// </summary>
        public static string GetVideoComment { get { return MangGuoHost + "mobile_comment/top?" + CommonUrl + "&type=hunantv2014"; } }
        /// <summary>
        /// 更多评论
        /// </summary>
        public static string GetMoreVideoComment { get { return MangGuoHost + "comment/read?" + CommonUrl + "&pageSize=30"; } } 
        /// <summary>
        /// 视频花絮
        /// </summary>
        public static string GetVideoRrelated { get { return MangGuoHost + "video/relatedVideos?" + CommonUrl; } }
        /// <summary>
        /// 获取更多频道信息
        /// </summary>
        public static string GetMoreChannelInfo { get { return MangGuoHost + "list?" + CommonUrl + "&pageSize=30"; } }
        /// <summary>
        /// 热搜
        /// </summary>
        public static string HotSearch { get { return MangGuoHost + "search/hotWords"; } }
        /// <summary>
        /// 搜索
        /// </summary>
        public static string SearchUrl { get { return MangGuoHost + "search/autocomplete?" + CommonUrl; } }
        /// <summary>
        /// 专题页面
        /// </summary>
        public static string SpecialPageName { get { return "/Views/MoreSubject.xaml"; } }
        /// <summary>
        /// 播放+详情页面
        /// </summary>
        public static string PlayerPageName { get { return "/Views/PlayerInfo.xaml"; } }
        /// <summary>
        /// 更多渠道信息
        /// </summary>
        public static string MoreChannelPageName { get { return "/Views/MoreChannelInfo.xaml"; } }
        /// <summary>
        /// 显示下载页面
        /// </summary>
        public static string DownVideoPage { get { return "/Views/DownVideo.xaml"; } }
        /// <summary>
        /// 主页
        /// </summary>
        public static string MianPage { get { return "/Views/MainPage.xaml"; } }
        /// <summary>
        /// 播放记录页面
        /// </summary>
        public static string RememberPage { get { return "/Views/RememberVideos.xaml"; } }
        /// <summary>
        /// 关于
        /// </summary>
        public static string AboutPage { get { return "/Views/About.xaml"; } }
        /// <summary>
        /// 设置
        /// </summary>
        public static string SettingPage { get { return "/Views/Setting.xaml"; } }
        /// <summary>
        /// 搜索页面
        /// </summary>
        public static string SearchPage { get { return "/Views/Search.xaml"; } }
        /// <summary>
        /// 直播
        /// </summary>
        public static string LivePlayerPage { get { return "/Views/LivePlayer.xaml"; } }
        


        #endregion

        #region IsolatedStorage FileName Or KeyName

        /// <summary>
        /// 播放记录存放位置
        /// </summary>
        public static string IsoRootPath { get { return "MangGuoData\\"; } }
        /// <summary>
        /// 播放记录存放位置
        /// </summary>
        public static string rememberVideoSavePath { get { return IsoRootPath + "RememberVideos\\"; } }
        /// <summary>
        /// 缓存文件存放位置
        /// </summary>
        public static string videoSavePath { get { return IsoRootPath+"DownVideos\\"; } }

        /// <summary>
        /// 是否第一次使用该App
        /// </summary>
        public static string IsFirstUse { get { return "IsFirstUse"; } }
        /// <summary>
        /// 本地保存的频道列表
        /// </summary>
        public static string ChannelStorage { get { return "channels.dat"; } }

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




        public static string LoginUrl { get; set; }
    }
}
