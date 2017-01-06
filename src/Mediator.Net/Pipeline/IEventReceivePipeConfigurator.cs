using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IEventReceivePipeConfigurator : IPipeConfigurator<IReceiveContext<IEvent>>
    {
        IEventReceivePipe<IReceiveContext<IEvent>> Build();
    }
}
