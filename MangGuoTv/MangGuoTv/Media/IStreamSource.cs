﻿#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.TsParser;
using System;

namespace SM.Media
{
    public interface IStreamSource
    {
        float? BufferingProgress { get; }
        bool HasSample { get; }
        bool IsEof { get; }

        void FreeSample(TsPesPacket packet);
        TsPesPacket GetNextSample();
    }
}
