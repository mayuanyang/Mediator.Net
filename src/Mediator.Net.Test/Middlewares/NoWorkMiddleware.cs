using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class NoWorkMiddleware
    {
        public static void UseNoWorkMiddleware<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new NoWorkMiddlewareSpecification<TContext>());
        }
    }

    class NoWorkMiddlewareSpecification<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;
        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context)
        {
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}