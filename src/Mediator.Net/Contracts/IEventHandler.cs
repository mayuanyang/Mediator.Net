using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface IEventHandler<in TMessage> 
        where TMessage : IEvent
    {
        Task Handle(IReceiveContext<TMessage> context);
    }
}
