using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Autofac.Test.Middlewares
{
    public static class SimpleMiddleware1
    {
        public static void UseSimpleMiddleware1<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new SimpleMiddleware1Specification<TContext>());
        }
    }

    public class SimpleMiddleware1Specification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;
        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
            {
                RubishBin.Rublish.Add(new object());
            }

            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            return Task.FromResult(0);
        }
    }
}
