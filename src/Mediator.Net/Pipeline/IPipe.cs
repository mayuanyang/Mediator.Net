using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Pipeline
{
    public interface IPipe<in TContext> where TContext : IContext
    {
        Task Send(TContext context);
    }
}
