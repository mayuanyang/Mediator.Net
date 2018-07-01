using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface IReceiveContext<out TMessage> : 
        IContext<TMessage> 
        where TMessage : IMessage
    {
        Task PublishAsync(IEvent message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
