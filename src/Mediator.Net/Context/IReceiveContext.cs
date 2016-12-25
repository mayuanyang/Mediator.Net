using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public interface IReceiveContext : IContext
    {
        
    }
    public interface IReceiveContext<out TMessage> : 
        IReceiveContext, 
        IContext<TMessage> 
        where TMessage : IMessage
    {
        Task Publish(IEvent message);
    }
}
