#region 程序集 SM.TsParser.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.TsParser.dll
#endregion

using System;

namespace SM.TsParser
{
    public class TsPesPacket
    {
        public TimeSpan? DecodeTimestamp;
        public TimeSpan? Duration;
        public int Index;
        public int Length;
        public readonly int PacketId;
        public TimeSpan PresentationTimestamp;

        public TsPesPacket();

        public byte[] Buffer { get; }

        public void Clear();
        public override string ToString();
    }
}
