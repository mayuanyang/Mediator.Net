using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public interface IReceivePipeConfigurator : IPipeConfigurator<IReceiveContext<IMessage>>
    {
        IReceivePipe<IReceiveContext<IMessage>> Build();
    }
}
