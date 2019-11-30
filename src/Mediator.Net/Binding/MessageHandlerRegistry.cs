using System;
using System.Collections.Generic;

namespace Mediator.Net.Binding
{
    public class MessageHandlerRegistry
    {
        HashSet<MessageBinding> _messageBindings;

        public HashSet<MessageBinding> MessageBindings
        {
            get => _messageBindings ?? (_messageBindings = new HashSet<MessageBinding>());
            internal set => _messageBindings = value;
        }
        
    }
}
