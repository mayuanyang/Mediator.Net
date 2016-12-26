using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPublishPipe<TMessage, in TContext> :IPipe<TMessage, TContext>
        where TMessage : IEvent
        where TContext : IContext<TMessage>
    {
    }
}
