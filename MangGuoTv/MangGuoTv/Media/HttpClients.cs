#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SM.Media.Web
{
    public class HttpClients : IHttpClients, IDisposable
    {
        public static readonly MediaTypeWithQualityHeaderValue AcceptAnyHeader;
        public static readonly MediaTypeWithQualityHeaderValue AcceptMp2tHeader;
        public static readonly MediaTypeWithQualityHeaderValue AcceptMp3Header;
        public static readonly MediaTypeWithQualityHeaderValue AcceptMpegurlHeader;
        public static readonly MediaTypeWithQualityHeaderValue AcceptOctetHeader;

        public HttpClients(Uri referrer = null, ProductInfoHeaderValue userAgent = null, ICredentials credentials = null, CookieContainer cookieContainer = null);

        public virtual HttpClient RootPlaylistClient { get; }

        public virtual HttpClient CreateBinaryClient(Uri referrer);
        protected virtual HttpClientHandler CreateClientHandler();
        protected virtual HttpClient CreateHttpClient(Uri referrer);
        public virtual HttpClient CreatePlaylistClient(Uri referrer);
        protected virtual HttpClient CreatePlaylistHttpClient(Uri referrer);
        public virtual HttpClient CreateSegmentClient(Uri segmentPlaylist);
        public void Dispose();
        protected virtual void Dispose(bool disposing);
    }
}
