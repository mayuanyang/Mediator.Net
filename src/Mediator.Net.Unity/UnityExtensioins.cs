using Mediator.Net.Binding;
using Microsoft.Practices.Unity;

namespace Mediator.Net.Unity
{
    public class UnityExtensioins
    {
        public static void Configure(MediatorBuilder builder, IUnityContainer container)
        {
            container.RegisterInstance(typeof(MediatorBuilder), builder);
            container.RegisterType<IMediator, Mediator>(new InjectionFactory(x =>
            {
                var child = new UnityDependancyScope(container).BeginScope();
                return builder.Build(child);
            }));

            foreach (var binding in MessageHandlerRegistry.MessageBindings)
            {
                container.RegisterType(binding.HandlerType, binding.HandlerType);
            }

        }
    }
}
