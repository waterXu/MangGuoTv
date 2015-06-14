#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion
using System.Threading;
using System.Threading.Tasks;
using SM.Media.Content;

namespace SM.Media.MediaParser
{
    public interface IMediaParserFactory : IContentServiceFactory<IMediaParser, IMediaParserParameters>
    {
    }
    public interface IMediaParserParameters
    {
    }
    public interface IContentServiceFactory<TService, TParameter>
    {
        Task<TService> CreateAsync(TParameter parameter, ContentType contentType, CancellationToken cancellationToken);
    }
}
