#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Buffering;
using System;
using System.Collections.Generic;

namespace SM.Media.MediaParser
{
    public interface IMediaParser : IDisposable
    {
        bool EnableProcessing { get; set; }
        ICollection<IMediaParserMediaStream> MediaStreams { get; }
        TimeSpan StartPosition { get; set; }

        event EventHandler ConfigurationComplete;

        void FlushBuffers();
        void Initialize(IBufferingManager bufferingManager, Action<SM.TsParser.IProgramStreams> programStreamsHandler = null);
        void ProcessData(byte[] buffer, int offset, int length);
        void ProcessEndOfData();
    }
}
