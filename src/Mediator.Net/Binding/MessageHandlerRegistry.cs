using System;
using System.Collections.Generic;

namespace Mediator.Net.Binding
{
    public class MessageHandlerRegistry
    {
        [ThreadStatic] static IList<MessageBinding> _messageBindings;

        public static IList<MessageBinding> MessageBindings
        {
            get { return _messageBindings ?? (_messageBindings = new List<MessageBinding>()); }
            internal set { _messageBindings = value; }
        }
        
    }
}
