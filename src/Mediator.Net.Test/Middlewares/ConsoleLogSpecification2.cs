using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Test.Middlewares
{
    class ConsoleLogSpecification2<TContext, TMessage> : IPipeSpecification<TContext, TMessage> where TMessage : IMessage where TContext : IContext<TMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if(ShouldExecute(context))
                Console.WriteLine("Before 2");
            return Task.FromResult(0);
            
        }

        public Task ExecuteAfterConnect(TContext context)
        {
           if (ShouldExecute(context))
                Console.WriteLine("After 2");
            return Task.FromResult(0);
        }
    }
}
