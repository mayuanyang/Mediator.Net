using System.Collections.Generic;
using System.Collections.Specialized;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{

    public interface IPublishContext<TMessage> : IContext<TMessage>
        where TMessage : IEvent 
    {
        
    }
}
