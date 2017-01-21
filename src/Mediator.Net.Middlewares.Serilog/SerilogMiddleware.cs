using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Serilog.Events;

namespace Mediator.Net.Middlewares.Serilog
{
    public static class SerilogMiddleware
    {
        public static void UseSerilog<TContext>(this IPipeConfigurator<TContext> configurator, LogEventLevel logAsLevel, ILogger logger = null)
            where TContext : IContext<IMessage>
        {
            if (logger == null && configurator.DependancyScope == null)
            {
                throw new DependancyScopeNotConfiguredException($"{nameof(ILogger)} is not provided and IDependancyScope is not configured, Please ensure {nameof(ILogger)} is registered properly if you are using IoC container, otherwise please pass {nameof(ILogger)} as parameter");
            }
            logger = logger ?? configurator.DependancyScope.Resolve<ILogger>();
            
            configurator.AddPipeSpecification(new SerilogMiddlewareSpecification<TContext>(logger, logAsLevel));
        }
    }
}