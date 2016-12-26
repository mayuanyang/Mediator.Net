using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface ISendPipe<in TContext, TMessage> : IPipe<TContext, TMessage>
        where TMessage : ICommand
        where TContext : IContext<TMessage>
    {
        
    }
}
