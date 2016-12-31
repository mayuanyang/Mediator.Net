using Autofac;
using Autofac.Core;
using Mediator.Net.Binding;

namespace Mediator.Net.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterMediator(this ContainerBuilder containerBuilder, MediatorBuilder mediatorBuilder)
        {
            containerBuilder.RegisterInstance(mediatorBuilder).AsSelf().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<AutofacDependancyScope>().AsImplementedInterfaces();
            containerBuilder.Register(x =>
            {
                var resolver = x.Resolve<IDependancyScope>().BeginScope();
                return mediatorBuilder.Build(resolver);
                
            }).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            
            RegisterHandlers(containerBuilder);
            
            return containerBuilder;
        }

        private static void RegisterHandlers(ContainerBuilder containerBuilder)
        {
            foreach (var binding in MessageHandlerRegistry.MessageBindings)
            {
                containerBuilder.RegisterType(binding.HandlerType).AsSelf();
            }
        }
    }
}
