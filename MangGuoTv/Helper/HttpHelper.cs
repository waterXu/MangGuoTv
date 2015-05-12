using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Microsoft.Phone.Info;
using System.IO;
using Windows.Networking.Connectivity;
using MangGuoTv.Models;


namespace MangGuoTv
{
    class HttpHelper
    {
        public static string rightCode = "200";
        public static string SyncResultTostring(IAsyncResult syncResult)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)syncResult.AsyncState).EndGetResponse(syncResult);
                Stream stream = response.GetResponseStream();
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);

                string result = System.Text.UTF8Encoding.UTF8.GetString(data, 0, data.Length);
                return result;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncResultTostring" + e.Message);
                // todo   show  tip server not conn  如何检测是否联网
                return null;
            }
           
        }
        public static byte[] SyncResultToByte(IAsyncResult syncResult)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)syncResult.AsyncState).EndGetResponse(syncResult);
                Stream stream = response.GetResponseStream();
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                return data;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncResultTostring" + e.Message);
                return null;
            }

        }
        public static void LoadChannelList()
        {
            bool IsLoaded = false;
            System.Diagnostics.Debug.WriteLine("LoadChannelList url:" + CommonData.VideoListUrl);
            httpGet(CommonData.VideoListUrl,(ar) => 
            {
                string result = HttpHelper.SyncResultTostring(ar);
                if (result != null)
                {
                    ChannelsResult channels = null;
                    try
                    {
                         channels = JsonConvert.DeserializeObject<ChannelsResult>(result);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("LoadChannelCompleted   json 解析错误"+ex.Message);
                    }
                    if (channels != null && channels.err_code == rightCode) 
                    {
                        CommonData.LockedChannel = channels.data.lockedChannel;
                        CommonData.NormalChannel = channels.data.normalChannel;
                        IsLoaded = true;
                        WpStorage.SaveStringToIsoStore(CommonData.ChannelStorage, JsonConvert.SerializeObject(channels.data));
                        CommonData.ChannelLoaded = true;
                    }
                }
                //CommonData.informCallback((int)CommonData.CallbackType.LoadedData, IsLoaded);
            });
        }
        /// <summary>
        /// HttpGet功能函数
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="asyncCallback">请求返回</param>
        public static void httpGet(string url, AsyncCallback asyncCallback,string cookieTag = null)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                IAsyncResult token = req.BeginGetResponse(asyncCallback, req);
            }
            catch { }
        }
        /// <summary>
        /// HttpGet功能函数
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="asyncCallback">请求返回</param>
        public static void httpGetDownFile(string url, AsyncCallback asyncCallback, Action<int, int> updateProgress = null)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                IAsyncResult token = req.BeginGetResponse(asyncCallback, req);
                long totalBytes = req.ContentLength; 
                if (updateProgress != null) 
                {
                    updateProgress((int)totalBytes, 0);
                }
            }
            catch { }
        }
        /// <summary>
        /// HttpPsot功能函数
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="asyncCallback">请求返回</param>
        public static void httpPost(string url, AsyncCallback asyncCallback)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                //req.Headers["Cache-Control"] = "no-cache";
                //req.Headers["Pragma"] = "no-cache";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36";
                //req.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                req.AllowAutoRedirect = true;
                IAsyncResult token = req.BeginGetResponse(asyncCallback, req);
            }
            catch { }
        }

        internal static bool LoginResultCodeInfo(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
    }
}
