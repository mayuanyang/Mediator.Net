using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class EmptyPipeSpecification<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage> 
    {
        public bool ShouldExecute(TContext context)
        {
            return false;
        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            return Task.FromResult(0);
        }

    }
}