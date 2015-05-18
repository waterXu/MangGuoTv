#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Buffering;
using SM.Media.MediaParser;
using System;
using System.Runtime.CompilerServices;

namespace SM.Media
{
    public static class MediaStreamFascadeExtensions
    {
        public static void SetParameter(this IMediaStreamFascade mediaStreamFascade, IBufferingPolicy policy);
        public static void SetParameter(this IMediaStreamFascade mediaStreamFascade, IMediaElementManager mediaElementManager);
        public static void SetParameter(this IMediaStreamFascade mediaStreamFascade, IMediaManagerParameters parameters);
        public static void SetParameter(this IMediaStreamFascade mediaStreamFascade, IMediaStreamSource mediaStreamSource);
        public static void SetParameter(this IMediaStreamFascade mediaStreamFascade, IPlaylistSegmentManagerParameters parameters);
    }
    public interface IPlaylistSegmentManagerParameters
    {
    }
}
