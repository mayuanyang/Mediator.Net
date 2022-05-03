using System.Collections.Generic;
using System.Threading;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts;

public interface IStreamCommandHandler<in TCommand, out TResponse>
    where TCommand : class, ICommand
    where TResponse : class, IResponse
{
    IAsyncEnumerable<TResponse> Handle(IReceiveContext<TCommand> context, CancellationToken cancellationToken);
}