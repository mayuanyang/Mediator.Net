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
            containerBuilder.RegisterType<AutofacDependencyScope>().AsImplementedInterfaces();
            containerBuilder.Register(x =>
            {
                var resolver = x.Resolve<IDependencyScope>();
                return mediatorBuilder.Build(resolver);
                
            }).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            
            RegisterHandlers(containerBuilder, mediatorBuilder);
            
            return containerBuilder;
        }

        private static void RegisterHandlers(ContainerBuilder containerBuilder, MediatorBuilder builder)
        {
            foreach (var binding in builder.MessageHandlerRegistry.MessageBindings)
            {
                containerBuilder.RegisterType(binding.HandlerType).AsSelf();
            }
        }
    }
}
