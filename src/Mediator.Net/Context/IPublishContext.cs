using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface IPublishContext : IContext
    {
        
    }
    public interface IPublishContext<out TMessage> : 
        IPublishContext, 
        IContext<TMessage> 
        where TMessage : IEvent
    {
    }
}
