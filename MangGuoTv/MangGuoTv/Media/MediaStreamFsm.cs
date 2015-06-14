#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;

namespace SM.Media.Utility
{
    public struct MediaStreamFsm
    {

        public void Reset();
        public override string ToString();
        public void ValidateEvent(MediaStreamFsm.MediaEvent mediaEvent);

        public enum MediaEvent
        {
            MediaStreamSourceAssigned = 0,
            OpenMediaAsyncCalled = 1,
            CallingReportOpenMediaCompleted = 2,
            CallingReportOpenMediaCompletedLive = 3,
            SeekAsyncCalled = 4,
            CallingReportSeekCompleted = 5,
            CallingReportSampleCompleted = 6,
            GetSampleAsyncCalled = 7,
            CloseMediaCalled = 8,
            MediaStreamSourceCleared = 9,
            DisposeCalled = 10,
            StreamsClosed = 11,
        }

        public enum MediaState
        {
            Invalid = 0,
            Idle = 1,
            Assigned = 2,
            Opening = 3,
            AwaitSeek = 4,
            Seeking = 5,
            AwaitPlaying = 6,
            Playing = 7,
            Closing = 8,
            Disposing = 9,
            Draining = 10,
        }
    }
}
