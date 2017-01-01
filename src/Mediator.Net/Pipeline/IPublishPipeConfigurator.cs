using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IPublishPipeConfigurator : IPipeConfigurator<IPublishContext<IEvent>>

    {
        IPublishPipe<IPublishContext<IEvent>> Build();
    }
}
