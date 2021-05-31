using System;
using System.Collections.Generic;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
 
    public interface IContext<out TMessage> where TMessage : IMessage
    {
        TMessage Message { get; }
        void RegisterService<T>(T service);
        bool TryGetService<T>(out T service);
        Dictionary<string, object> MetaData { get; }
        object Result { get; set; }
        
    }
}
