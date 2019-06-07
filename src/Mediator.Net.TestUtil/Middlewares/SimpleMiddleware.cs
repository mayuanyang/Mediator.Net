using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.Services;

namespace Mediator.Net.TestUtil.Middlewares
{
    public static class SimpleMiddleware
    {
        public static void UseSimpleMiddleware<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new SimpleMiddlewareSpecification<TContext>());
        }
    }

    public class SimpleMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return true;
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                Console.WriteLine($"Before 1: {context.Message}");
                context.RegisterService(new DummyTransaction());
            }
            return Task.FromResult(0);
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
                Console.WriteLine($"After 1: {context.Message}");
            return Task.FromResult(0);
        }

        public Task OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}
