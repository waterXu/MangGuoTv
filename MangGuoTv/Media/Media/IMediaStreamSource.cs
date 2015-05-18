#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media;
using SM.Media.Utility;
using System;
using System.Threading.Tasks;

namespace SM.Media.MediaParser
{
    public interface IMediaStreamSource : IDisposable
    {
        IMediaManager MediaManager { get; set; }
        TimeSpan? SeekTarget { get; set; }

        void CheckForSamples();
        Task CloseAsync();
        void Configure(IMediaConfiguration configuration);
        void ReportError(string message);
        void ValidateEvent(MediaStreamFsm.MediaEvent mediaEvent);
    }
}
