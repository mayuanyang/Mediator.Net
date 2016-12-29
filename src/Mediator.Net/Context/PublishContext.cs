using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    class PublishContext : IPublishContext
    {
        public PublishContext(IEvent message, IReceiveContext<IMessage> receiveContext )
        {
            Message = message;
            ReceiveContext = receiveContext;
        }
        public IEvent Message { get; }
        public IReceiveContext<IMessage> ReceiveContext { get; }
    }
}