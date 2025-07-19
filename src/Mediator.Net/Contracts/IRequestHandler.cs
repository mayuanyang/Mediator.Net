using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts;

public interface IRequestHandler<in TRequest,TResponse>
    where TRequest : class, IRequest
    where TResponse : class , IResponse
{
    Task<TResponse> Handle(IReceiveContext<TRequest> context, CancellationToken cancellationToken);
}