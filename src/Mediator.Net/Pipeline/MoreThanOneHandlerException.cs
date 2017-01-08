using System;

namespace Mediator.Net.Pipeline
{
    public class MoreThanOneHandlerException : Exception
    {
        public MoreThanOneHandlerException(Type getType) 
            : base ($"Cannot have more than one handler for message type {getType.FullName}")
        {
            
        }
    }
}