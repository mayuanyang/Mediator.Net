using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Test.Middlewares
{
    static class DummySave
    {
        public static void UseDummySave<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new DummySaveSpecification<TContext>());
        }
    }

    class DummySaveSpecification<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("Before save to DB");
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("After save to DB");
            return Task.FromResult(0);
        }
    }
}
