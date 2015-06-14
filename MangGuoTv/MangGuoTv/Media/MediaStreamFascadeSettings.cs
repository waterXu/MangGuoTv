#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;
using SM.Media.Buffering;
using System.Net.Http;
using System.Threading.Tasks;
using SM.Media.Content;
using SM.Media.Builder;
namespace SM.Media
{
    public class MediaStreamFascadeSettings
    {
        public MediaStreamFascadeSettings();

        public static MediaStreamFascadeParameters Parameters { get; set; }
    }
    public class MediaStreamFascadeParameters
    {
        public MediaStreamFascadeParameters();

        public IBufferingPolicy BufferingPolicy { get; set; }
        //public Func<Web.IHttpClients, Func<MediaParser.IMediaStreamSource, Windows.System.Threading.Tasks.Task>, IMediaStreamFascade> Factory { get; set; }
    }
    public interface IMediaStreamFascade : IDisposable
    {
        IBuilder<IMediaManager> Builder { get; }
        ContentType ContentType { get; set; }
        TimeSpan? SeekTarget { get; set; }
        Uri Source { get; set; }
        TsMediaManager.MediaState State { get; }

        event EventHandler<TsMediaManagerStateEventArgs> StateChange;

        Task CloseAsync();
        void Play();
        void RequestStop();
    }
  
    public interface IBufferingPolicy
    {
        float GetProgress(TimeSpan bufferDuration, int bytesBuffered, int bytesBufferedWhenExhausted, bool isStarting);
        bool IsDoneBuffering(TimeSpan bufferDuration, int bytesBuffered, int bytesBufferedWhenExhausted, bool isStarting);
        bool ShouldBlockReads(bool isReadBlocked, TimeSpan durationBuffered, int bytesBuffered, bool isExhausted, bool isAllExhausted);
    }
}
