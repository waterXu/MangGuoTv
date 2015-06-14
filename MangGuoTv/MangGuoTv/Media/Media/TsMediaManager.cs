#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Content;
using SM.Media.MediaParser;
using SM.Media.Segments;
using SM.Media.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SM.Media
{
    public sealed class TsMediaManager : IMediaManager, IDisposable
    {
        public TsMediaManager(ISegmentReaderManagerFactory segmentReaderManagerFactory, IMediaElementManager mediaElementManager, IMediaStreamSource mediaStreamSource, Func<Buffering.IBufferingManager> bufferingManagerFactory, IMediaManagerParameters mediaManagerParameters, IMediaParserFactory mediaParserFactory, IPlatformServices platformServices);

        public ContentType ContentType { get; set; }
        public IMediaStreamSource MediaStreamSource { get; }
        public TimeSpan? SeekTarget { get; set; }
        public ICollection<Uri> Source { get; set; }
        public TsMediaManager.MediaState State { get; }

        public event EventHandler<TsMediaManagerStateEventArgs> OnStateChange;

        [DebuggerStepThrough]
        public Task CloseAsync();
        public void CloseMedia();
        public void Dispose();
        public void OpenMedia();
        [DebuggerStepThrough]
        public Task<TimeSpan> SeekMediaAsync(TimeSpan position);

        public enum MediaState
        {
            Idle = 0,
            Opening = 1,
            OpenMedia = 2,
            Seeking = 3,
            Playing = 4,
            Closed = 5,
            Error = 6,
            Closing = 7,
        }
    }
}
