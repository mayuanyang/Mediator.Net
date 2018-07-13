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
            
            RegisterHandlers(container);
           
        }

        private static void RegisterHandlers(Container containerBuilder)
        {
            foreach (var binding in MessageHandlerRegistry.MessageBindings)
            {
                containerBuilder.Register(binding.HandlerType);
            }
        }
    }
}
