using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IReceivePipe<TMessage, in TContext> : IPipe<TMessage, TContext> 
        where TMessage : IMessage
        where TContext : IContext<TMessage>
    {
        
    }
}
