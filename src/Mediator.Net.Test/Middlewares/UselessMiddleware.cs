using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Test.Middlewares
{
    static class UselessMiddleware
    {
        public static void UseUselessMiddleware<TContext, TMessage>(this IPipeConfigurator<TContext, TMessage> configurator)
            where TContext : IContext<TMessage>
            where TMessage : IMessage
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification1<TContext, TMessage>());
        }
    }

    class UselessMiddlewareSpecification<TContext, TMessage> : IPipeSpecification<TContext, TMessage> where TMessage : IMessage where TContext : IContext<TMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return false;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"you should never see me: {nameof(ExecuteBeforeConnect)}");
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"you should never see me: {nameof(ExecuteAfterConnect)}");
            return Task.FromResult(0);
        }
    }
}