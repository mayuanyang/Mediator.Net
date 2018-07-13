using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.TestUtils;
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
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return false;
        }

        public Task ExecuteBeforeConnect(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                Console.WriteLine($"you should never see me: {nameof(ExecuteBeforeConnect)}");
                RubishBox.Rublish.Add(nameof(UselessMiddleware.UseUselessMiddleware));
            }

            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
                Console.WriteLine($"you should never see me: {nameof(ExecuteAfterConnect)}");
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}