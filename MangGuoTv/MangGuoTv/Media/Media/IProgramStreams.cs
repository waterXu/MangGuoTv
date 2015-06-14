#region 程序集 SM.TsParser.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.TsParser.dll
#endregion

using System;
using System.Collections.Generic;

namespace SM.TsParser
{
    public interface IProgramStreams
    {
        int ProgramNumber { get; }
        ICollection<IProgramStream> Streams { get; }
    }
    public interface IProgramStream
    {
        bool BlockStream { get; set; }
        uint Pid { get; }
        TsStreamType StreamType { get; }
    }
    public class TsStreamType : IEquatable<TsStreamType>
    {
        public static readonly byte AacStreamType;
        public static readonly byte Ac3StreamType;
        public static readonly byte H262StreamType;
        public static readonly byte H264StreamType;
        public static readonly byte Mp3Iso11172;
        public static readonly byte Mp3Iso13818;

        public TsStreamType(byte streamType, TsStreamType.StreamContents contents, string description);

        public TsStreamType.StreamContents Contents { get; }
        public string Description { get; }
        public string FileExtension { get; }
        public byte StreamType { get; }

        public override bool Equals(object obj);
        public bool Equals(TsStreamType other);
        public static TsStreamType FindStreamType(byte streamType);
        public override int GetHashCode();
        public override string ToString();

        public enum StreamContents
        {
            Unknown = 0,
            Audio = 1,
            Video = 2,
            Other = 3,
            Private = 4,
            Reserved = 5,
        }
    }
}
