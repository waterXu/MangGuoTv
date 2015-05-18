#region 程序集 SM.Media.dll, v1.2.2.0
// C:\Users\xyl\Documents\《Windows+Phone8开发技巧与案例精解》随书源码\phonesm-1.2.2\phonesm-1.2.2\bin\Debug\SM.Media.dll
#endregion

using System;

namespace SM.Media.Builder
{
    public interface IBuilder<TBuild> : IBuilder, IDisposable
    {
        TBuild Create();
        void Destroy(TBuild instance);
    }
    public interface IBuilder : IDisposable
    {
        void Register<TService, TImplementation>() where TImplementation : TService;
        void RegisterFactory<TService>(Func<TService> factory);
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;
        void RegisterSingleton<TService>(TService instance) where TService : class;
    }
}
