using Ninject;

namespace Mediator.Net.Ninject
{
    public static class NinjectExtensions
    {
        public static void Configure(StandardKernel kernal, MediatorBuilder builder)
        {
            kernal.Bind<MediatorBuilder>().ToConstant(builder);
            kernal.Bind<IDependencyScope>().ToMethod(x => new NinjectDependencyScope(kernal));
        }
    }
}
