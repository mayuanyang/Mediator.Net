using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipe<in TContext, TMessage> 
        where TContext : IContext<TMessage> 
        where TMessage : IMessage
    {
        Task Connect(TContext context);
    }
}
