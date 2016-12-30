using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IRequestPipeConfigurator<TContext> : IPipeConfigurator<TContext>
        where TContext : IReceiveContext<IRequest>
    {
        IRequestPipe<TContext> Build();
    }
}
