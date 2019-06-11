using Mediator.Net.Binding;
using SimpleInjector;

namespace Mediator.Net.SimpleInjector
{
    public static class ContainerExtensions
    {
        public static void RegisterMediator(this Container container, MediatorBuilder mediatorBuilder)
        {
            container.RegisterSingleton(mediatorBuilder);
            container.Register(() =>
            {
                var resolver = new SimpleInjectorDependencyScope(container);
                return mediatorBuilder.Build(resolver);
            }, Lifestyle.Scoped);
            
            RegisterHandlers(container, mediatorBuilder);
           
        }

        private static void RegisterHandlers(Container containerBuilder, MediatorBuilder builder)
        {
            foreach (var binding in builder.MessageHandlerRegistry.MessageBindings)
            {
                containerBuilder.Register(binding.HandlerType);
            }
        }
    }
}
