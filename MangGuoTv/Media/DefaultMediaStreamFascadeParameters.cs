#region 程序集 SM.Media.Platform.WP8.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\WP8\ARM\SM.Media.Platform.WP8.dll
#endregion

using SM.Media.Web;
using System;
using System.Runtime.CompilerServices;

namespace SM.Media
{
    public static class DefaultMediaStreamFascadeParameters
    {
        public static Func<IHttpClients, Func<SM.Media.MediaParser.IMediaStreamSource, System.Threading.Tasks.Task>, IMediaStreamFascade> Factory;

        public static IMediaStreamFascade Create(this MediaStreamFascadeParameters parameters, IHttpClients httpClients, Func<SM.Media.MediaParser.IMediaStreamSource, System.Threading.Tasks.Task> mediaStreamSourceSetter);
    }
}
