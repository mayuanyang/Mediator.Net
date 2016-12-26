using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
 
    public interface IContext<out TMessage> where TMessage : IMessage
    {
        TMessage Message { get; }
    }
}
