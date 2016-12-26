using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Net.Binding
{
    class MessageBinding
    {
        public Type MessageType { get; }
        public Type HandlerType { get; }

        public MessageBinding(Type messageType, Type handlerType)
        {
            MessageType = messageType;
            HandlerType = handlerType;
        }
    }
}
