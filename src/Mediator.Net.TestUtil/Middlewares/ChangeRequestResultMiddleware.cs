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
    public static class ChangeRequestResultMiddleware
    {
        public static void UseChangeRequestResultMiddleware<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new ChangeRequestResultMiddlewareSpecification<TContext>());
        }
    }

    public class ChangeRequestResultMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return true;
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            (context.Result as dynamic).ToBeSetByMiddleware = "i am from middleware";
            return Task.CompletedTask;
        }


        public Task OnException(Exception ex, TContext context)
        {
            RubishBox.Rublish.Add(ex);
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw ex;
        }
    }
}