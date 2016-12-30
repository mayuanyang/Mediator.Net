using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface IRequestHandler<TRequest>
        where TRequest : class, IRequest
    {
        Task<object> Handle(ReceiveContext<TRequest> context);
    }
}
