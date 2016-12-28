using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IReceivePipe<in TContext> : IPipe<TContext> 
        where TContext : IContext<IMessage>
    {
        
    }
}
