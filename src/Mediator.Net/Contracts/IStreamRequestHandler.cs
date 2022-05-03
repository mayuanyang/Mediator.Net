using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface IStreamRequestHandler<in TRequest, out TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        IAsyncEnumerable<TResponse> Handle(IReceiveContext<TRequest> context, CancellationToken cancellationToken);
    }
}