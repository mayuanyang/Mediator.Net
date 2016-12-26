using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{

    public interface IPublishContext<out TMessage> : 
        IContext<TMessage> 
        where TMessage : IEvent
    {
    }
}
