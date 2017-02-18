using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class MiddlewareThrowExAfterConnect
    {
        public static void UseMiddlewareThrowExAfterConnect<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new MiddlewareThrowExAfterConnectSpecification<TContext>());
        }
    }

    class MiddlewareThrowExAfterConnectSpecification<TContext> : IPipeSpecification<TContext> 
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

        public Task ExecuteAfterConnect(TContext context)
        {
            throw new Exception();
        }

        public void OnException(Exception ex, TContext context)
        {
            RubishBox.Rublish.Add(ex);
            throw ex;
        }
    }
}
