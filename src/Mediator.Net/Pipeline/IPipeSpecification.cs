using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        bool ShouldExecute(TContext context, CancellationToken cancellationToken);
        Task ExecuteBeforeConnect(TContext context, CancellationToken cancellationToken);
        Task Execute(TContext context, CancellationToken cancellationToken);
        Task ExecuteAfterConnect(TContext context, CancellationToken cancellationToken);
        void OnException(Exception ex, TContext context);
    }
}