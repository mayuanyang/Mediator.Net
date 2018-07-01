using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IRequestReceivePipe<TContext> : IPipe<TContext>
        where TContext : IContext<IRequest>
    {
        new Task<object> Connect(TContext context, CancellationToken cancellationToken);
    }
}
