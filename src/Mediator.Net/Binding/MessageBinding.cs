using System;

namespace Mediator.Net.Binding;

public class MessageBinding
{
    public Type MessageType { get; }
    public Type HandlerType { get; }

    public MessageBinding(Type messageType, Type handlerType)
    {
        MessageType = messageType;
        HandlerType = handlerType;
    }
}