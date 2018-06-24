using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.Services;
using Mediator.Net.Pipeline;

namespace Mediator.Net.IoCTestUtil.Middlewares
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
            if (ShouldExecute(context))
            {
                Console.WriteLine($"Before 1: {context.Message}");
                context.RegisterService(new DummyTransaction());
            }
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"After 1: {context.Message}");
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}
