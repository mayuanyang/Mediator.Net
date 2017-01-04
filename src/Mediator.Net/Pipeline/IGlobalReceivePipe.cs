using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IGlobalReceivePipe<TContext> : IPipe<TContext>
        where TContext : IContext<IMessage>
    {
       
    }
}
