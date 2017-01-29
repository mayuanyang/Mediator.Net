using Ninject;

namespace Mediator.Net.Ninject
{
    public static class NinjectExtensions
    {
        public static void Configure(StandardKernel kernal, MediatorBuilder builder)
        {
            kernal.Bind<MediatorBuilder>().ToConstant(builder);
            kernal.Bind<IDependancyScope>().ToMethod(x => new NinjectDependancyScope(kernal));
        }
    }
}
