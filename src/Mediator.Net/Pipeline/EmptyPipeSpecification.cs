using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class EmptyPipeSpecification<TContext, TMessage> : IPipeSpecification<TContext, TMessage> 
        where TContext : IContext<TMessage> 
        where TMessage : IMessage
    {
        public bool ShouldExecute(TContext context)
        {
            return false;
        }

        public Task ExecuteBeforeConnect(TContext context)
        {
#if DEBUG
            Console.WriteLine($"empty specification {nameof(ExecuteBeforeConnect)}");
#endif
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
#if DEBUG
            Console.WriteLine($"empty specification {nameof(ExecuteAfterConnect)}");
#endif
            return Task.FromResult(0);
        }

    }
}