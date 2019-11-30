using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Middlewares
{
    public static class MiddlewareThrowExBeforeConnect
    {
        public static void UseMiddlewareThrowExBeforeConnect<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new MiddlewareThrowExBeforeConnectSpecification<TContext>());
        }
    }

    public class MiddlewareThrowExBeforeConnectSpecification<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return true;
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            throw new Exception();
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
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
            RubishBox.Rublish.Add(ex);
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw ex;
        }
    }
}
