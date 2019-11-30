using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.Autofac.Test.Middlewares
{
    public static class EnrichResultOnExceptionMiddleware
    {
        public static void UseEnrichResultOnException<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new EnrichResultOnExceptionMiddlewareSpecification<TContext>());
        }
    }

    public class EnrichResultOnExceptionMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
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
                RubishBin.Rublish.Add(new object());
            }
            return Task.FromResult(0);
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task OnException(Exception ex, TContext context)
        {
            return Task.FromResult(new SimpleResponse("Error has occured"));
        }
    }
}
