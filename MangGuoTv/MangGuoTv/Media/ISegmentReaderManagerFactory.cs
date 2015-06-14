#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using SM.Media.Content;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Media.Segments
{
    public interface ISegmentReaderManagerFactory
    {
        Task<ISegmentReaderManager> CreateAsync(ICollection<System.Uri> source, ContentType contentType, CancellationToken cancellationToken);
    }
}
