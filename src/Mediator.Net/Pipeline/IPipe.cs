﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline;

public interface IPipe<in TContext> 
    where TContext : IContext<IMessage> 
{
    Task<object> Connect(TContext context, CancellationToken cancellationToken);

    IAsyncEnumerable<TResponse> ConnectStream<TResponse>(TContext context, CancellationToken cancellationToken);
    
    IPipe<TContext> Next { get; }
}