using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface ISendContext<out TMessage> : 
        IContext<TMessage> 
        where TMessage : ICommand
    {
    }
}
