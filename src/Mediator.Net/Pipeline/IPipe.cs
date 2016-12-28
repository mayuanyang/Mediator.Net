using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipe<in TContext> 
        where TContext : IContext<IMessage> 
    {
        Task Connect(TContext context);
        IPipe<TContext> Next { get; }

    }
}
