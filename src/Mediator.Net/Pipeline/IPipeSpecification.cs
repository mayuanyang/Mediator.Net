using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipeSpecification<TContext, TMessage>
        where TContext : IContext<TMessage>
        where TMessage : IMessage
    {
    }
}