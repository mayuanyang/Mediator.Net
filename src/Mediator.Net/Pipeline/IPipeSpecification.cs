using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline;

public interface IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    bool ShouldExecute(TContext context, CancellationToken cancellationToken);

    Task BeforeExecute(TContext context, CancellationToken cancellationToken);

    Task Execute(TContext context, CancellationToken cancellationToken);

    Task AfterExecute(TContext context, CancellationToken cancellationToken);

    Task OnException(Exception ex, TContext context);
}