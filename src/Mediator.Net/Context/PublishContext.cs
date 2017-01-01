using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    class PublishContext : IPublishContext<IEvent>
    {
       
        public PublishContext(IEvent message, IMediator mediator)
        {
            Mediator = mediator;
            Message = message;
        }
        public IEvent Message { get; }
        public IMediator Mediator { get; }
    }
}