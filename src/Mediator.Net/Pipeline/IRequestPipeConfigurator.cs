using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IRequestPipeConfigurator : IPipeConfigurator<IReceiveContext<IRequest>>
    {
        IRequestPipe<IReceiveContext<IRequest>, IResponse> Build();
    }
}
