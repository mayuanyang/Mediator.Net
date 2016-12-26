using System;

namespace Mediator.Net.Pipeline
{
    public class NoHandlerFoundException : Exception
    {
        public NoHandlerFoundException(Type getType) : base($"No handler found for message type {getType.FullName}")
        {
            
        }
    }
}