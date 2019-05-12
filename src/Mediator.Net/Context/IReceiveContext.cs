using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface IReceiveContext<out TMessage> : 
        IContext<TMessage> 
        where TMessage : IMessage
    {
        TMessage Message { get; }
        Dictionary<string, object> MetaData { get; }
        void RegisterService<T>(T service);
        bool TryGetService<T>(out T service);
        Task PublishAsync(IEvent message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
