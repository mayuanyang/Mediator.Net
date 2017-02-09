using Mediator.Net.Binding;
using SimpleInjector;

namespace Mediator.Net.SimpleInjector
{
    public static class InjectHelper
    {
        public static void RegisterMediator(Container container, MediatorBuilder mediatorBuilder)
        {
            container.RegisterSingleton<MediatorBuilder>(mediatorBuilder);
            container.Register(() =>
            {
                var resolver = new SimpleInjectorDependancyScope(container);
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
