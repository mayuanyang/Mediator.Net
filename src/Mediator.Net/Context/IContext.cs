using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface IContext
    {
        
    }
    public interface IContext<out TMessage> : IContext where TMessage : IMessage
    {
        TMessage Message { get; }
    }
}
