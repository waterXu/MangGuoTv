using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MangGuoTv.ViewModels
{
    public class VideoViewModel:ViewModelBase
    {
        public int width { get; set; }
        public int hight { get; set; }
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
        public string tag { get; set; }
        public string desc { get; set; }
        /// <summary>
        /// 视频id
        /// </summary>
        public string videoId { get; set; }
        public string hotDegree { get; set; }
        public string webUrl { get; set; }
        public string rank { get; set; }
        public string playCount { get; set; }
        // 若引用对象，用于存储下载好的图片对象
        WeakReference bitmapImage;
        // ImageSource属性用于绑定到列表的Image控件上
        public ImageSource ImageSource
        {
            get
            {
                if (bitmapImage != null)
                {
                    // 如果弱引用没有没回收，则取弱引用的值
                    if (bitmapImage.IsAlive)
                        return (ImageSource)bitmapImage.Target;
                    else
                        Debug.WriteLine("数据已经被回收");
                }
                // 弱引用已经被回收那么则通过图片网络地址进行异步下载
                if (!string.IsNullOrEmpty( picUrl))
                {
                    Task.Factory.StartNew(() => { DownloadImage(picUrl); });
                }
                return null;
            }
        }
        // 下载图片的方法
        void DownloadImage(string url)
        {
            //HttpWebRequest request = WebRequest.CreateHttp( new Uri(url,UriKind.RelativeOrAbsolute));
            //request.BeginGetResponse(DownloadImageComplete, request);
            HttpHelper.httpGet(url, DownloadImageComplete);
        }
        // 完成图片下载的回调方法
        void DownloadImageComplete(IAsyncResult result)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)result.AsyncState).EndGetResponse(result);
                Stream stream = response.GetResponseStream();
                int length = int.Parse(response.Headers["Content-Length"]);
                // 注意需要把数据流重新复制一份，否则会出现跨线程错误
                // 网络下载到的图片数据流，属于后台线程的对象，不能在UI上使用
                Stream streamForUI = new MemoryStream(length);
                byte[] buffer = new byte[length];
                int read = 0;
                do
                {
                    read = stream.Read(buffer, 0, length);
                    streamForUI.Write(buffer, 0, read);
                } while (read == length);
                streamForUI.Seek(0, SeekOrigin.Begin);
                // 触发UI线程处理位图和UI更新
                CallbackManager.currentPage.Dispatcher.BeginInvoke(() =>
                {
                    BitmapImage bm = new BitmapImage();
                    bm.DecodePixelHeight = hight*3/5;
                    bm.DecodePixelWidth = width * 4 / 5;
                    bm.SetSource(streamForUI);
                    // 把图片位图对象存放到若引用对象里面
                    if (bitmapImage == null)
                        bitmapImage = new WeakReference(bm);
                    else
                        bitmapImage.Target = bm;

                    //触发UI绑定属性的改变
                    NotifyPropertyChanged("ImageSource");
                }
                );
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncResultTostring" + e.Message);
            }
          
            
        }
    }
}
