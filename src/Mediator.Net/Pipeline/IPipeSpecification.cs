using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPipeSpecification<TContext, TMessage>
        where TContext : IContext<TMessage>
        where TMessage : IMessage
    {
        bool ShouldExecute(TContext context);

        Task ExecuteBeforeConnect(TContext context);
        Task ExecuteAfterConnect(TContext context);
    }
}