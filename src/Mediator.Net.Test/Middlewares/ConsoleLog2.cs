using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Test.Middlewares
{
    static class ConsoleLog2
    {
        public static void UseConsoleLogger2<TContext, TMessage>(this IPipeConfigurator<TContext, TMessage> configurator)
            where TContext : IContext<TMessage>
            where TMessage : IMessage
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification2<TContext, TMessage>());
        }
    }
}
