using Mediator.Net.Binding;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Net.MicrosoftDependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceCollection RegisterMediator(this IServiceCollection services, MediatorBuilder mediatorBuilder)
        {
            services.AddSingleton<MediatorBuilder>(mediatorBuilder);

            services.AddScoped(x =>
            {
                var scope = x.CreateScope();
                var dependencyScope = new MicrosoftDependencyScope(scope);
                return mediatorBuilder.Build(dependencyScope);
            });

            foreach (var binding in MessageHandlerRegistry.MessageBindings)
            {
                services.AddScoped(binding.HandlerType);
            }

            return services;
        }
    }
}