using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IReceivePipe<in TContext, TMessage> : IPipe<TContext, TMessage> 
        where TMessage : IMessage
        where TContext : IContext<TMessage>
    {
        
    }
}
