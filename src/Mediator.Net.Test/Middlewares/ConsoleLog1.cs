using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class ConsoleLog1
    {
        public static void UseConsoleLogger1<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification1<TContext>());
        }
    }

    class ConsoleLogSpecification1<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"Before 1: {context.Message}");
            RubishBox.Rublish.Add(nameof(ConsoleLog1.UseConsoleLogger1));
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"After 1: {context.Message}");
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            RubishBox.Rublish.Add(ex);
            throw ex;
        }
    }
}
