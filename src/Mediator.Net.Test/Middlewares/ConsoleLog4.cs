using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class ConsoleLog4
    {
        public static void UseConsoleLogger4<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification4<TContext>());
        }
    }

    class ConsoleLogSpecification4<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("Before 4");
            RubishBox.Rublish.Add(nameof(ConsoleLog4.UseConsoleLogger4));
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("After 4");
            return Task.FromResult(0);
        }
    }
}
