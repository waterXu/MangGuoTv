#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;

namespace SM.Media.Configuration
{
    public interface IConfigurationSource
    {
        int? Bitrate { get; }
        string CodecPrivateData { get; }
        bool IsConfigured { get; }
        string Name { get; }
        string StreamDescription { get; }

        event EventHandler ConfigurationComplete;
    }
}
