#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Content;
using SM.Media.Utility;
using System;
using System.Threading.Tasks;

namespace SM.Media.Segments
{
    public interface ISegmentManager : IStopClose, IDisposable
    {
        ContentType ContentType { get; }
        TimeSpan? Duration { get; }
        IAsyncEnumerable<ISegment> Playlist { get; }
        TimeSpan StartPosition { get; }
        Uri Url { get; }

        Task<TimeSpan> SeekAsync(TimeSpan timestamp);
        Task StartAsync();
    }
}
