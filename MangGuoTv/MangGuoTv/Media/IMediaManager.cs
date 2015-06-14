#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Content;
using SM.Media.MediaParser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SM.Media
{
    public interface IMediaManager : IDisposable
    {
        ContentType ContentType { get; set; }
        IMediaStreamSource MediaStreamSource { get; }
        TimeSpan? SeekTarget { get; set; }
        ICollection<Uri> Source { get; set; }
        TsMediaManager.MediaState State { get; }

        event EventHandler<TsMediaManagerStateEventArgs> OnStateChange;

        Task CloseAsync();
        void CloseMedia();
        void OpenMedia();
        Task<TimeSpan> SeekMediaAsync(TimeSpan position);
    }
}
