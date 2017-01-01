using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public class ReceiveContext<TMessasge> : IReceiveContext<TMessasge>
        where TMessasge : IMessage
    {
        private readonly IMediator _mediator;

        public ReceiveContext(TMessasge message, IMediator mediator)
        {
            _mediator = mediator;
            Message = message;
        }
        public TMessasge Message { get; }
        public Task PublishAsync(IEvent msg)
        {
            var publishContext = (IPublishContext<IEvent>)Activator.CreateInstance(typeof(PublishContext), msg, _mediator);
            var sendMethod = _mediator.PublishPipe.GetType().GetMethod("PublishAsync");
            var task = (Task)sendMethod.Invoke(_mediator.PublishPipe, new object[] { publishContext, _mediator });
            task.ConfigureAwait(false);
            return task;
        }
    }
}