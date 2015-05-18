#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Media.Utility
{
    public static class TplTaskExtensions
    {
        public static readonly Task CompletedTask;
        public static readonly Task<bool> FalseTask;
        public static readonly Task<bool> TrueTask;

        [DebuggerStepThrough]
        public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken);
        [DebuggerStepThrough]
        public static Task WithCancellation(this Task task, CancellationToken cancellationToken);
    }
}
