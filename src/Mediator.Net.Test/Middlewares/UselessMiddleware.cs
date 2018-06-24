using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class UselessMiddleware
    {
        public static void UseUselessMiddleware<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new UselessMiddlewareSpecification<TContext>());
        }
    }

    class UselessMiddlewareSpecification<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return false;
        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context)
        {
            if (ShouldExecute(context))
            {
                Console.WriteLine($"you should never see me: {nameof(ExecuteBeforeConnect)}");
                RubishBox.Rublish.Add(nameof(UselessMiddleware.UseUselessMiddleware));
            }

            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"you should never see me: {nameof(ExecuteAfterConnect)}");
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}