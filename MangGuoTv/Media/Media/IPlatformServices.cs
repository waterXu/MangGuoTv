#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;
using System.IO;

namespace SM.Media.Utility
{
    public interface IPlatformServices
    {
        Stream Aes128DecryptionFilter(Stream stream, byte[] key, byte[] iv);
        double GetRandomNumber();
    }
}
