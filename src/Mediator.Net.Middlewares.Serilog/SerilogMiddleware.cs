using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Serilog.Events;

namespace Mediator.Net.Middlewares.Serilog
{
    public static class SerilogMiddleware
    {
        public static void UseSerilog<TContext>(this IPipeConfigurator<TContext> configurator, ILogger logger, LogEventLevel logAsLevel)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new SerilogMiddlewareSpecification<TContext>(logger, logAsLevel));
        }
    }
}