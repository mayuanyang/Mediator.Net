using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface ISendPipe<in TContext> : IPipe<TContext>
        where TContext : IContext<ICommand>
    {
        
    }
}
