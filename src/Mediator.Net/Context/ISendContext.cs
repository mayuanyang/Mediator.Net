using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface ISendContext : IContext
    {
        
    }
    public interface ISendContext<out TMessage> : 
        ISendContext,
        IContext<TMessage> 
        where TMessage : ICommand
    {
    }
}
