using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipeConfigurator
    {
        
    }

    public interface IPipeConfigurator<TContext, TMessage> : IPipeConfigurator
        where TContext : IContext<TMessage>
        where TMessage : IMessage
    {
        void AddPipeSpecification(IPipeSpecification<TContext, TMessage> specification);

        IPipe<TContext, TMessage> Build();
    }
}
