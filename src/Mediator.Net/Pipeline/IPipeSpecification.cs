using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        bool ShouldExecute(TContext context);
        Task ExecuteBeforeConnect(TContext context);
        Task ExecuteAfterConnect(TContext context);
        void OnException(Exception ex, TContext context);
    }
}