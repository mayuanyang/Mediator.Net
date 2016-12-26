using System;

namespace Mediator.Net.Pipeline
{
    public class MoreThanOneCommandHandlerException : Exception
    {
        public MoreThanOneCommandHandlerException(Type getType) : base ($"Cannot have more than one command handler for message type {getType.FullName}")
        {
            
        }
    }
}