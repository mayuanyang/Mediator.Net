using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IRequestPipe<TContext, TResponse> : IPipe<TContext>
        where TContext : IContext<IRequest>
        where TResponse : IResponse
    {
        new Task<TResponse> Connect(TContext context);
    }
}
