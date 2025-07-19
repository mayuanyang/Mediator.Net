using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline;

public class EmptyPipeSpecification<TContext> : IPipeSpecification<TContext> 
    where TContext : IContext<IMessage> 
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return false;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
    
    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        throw ex;
    }
}