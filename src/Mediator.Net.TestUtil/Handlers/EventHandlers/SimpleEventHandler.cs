using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;

namespace Mediator.Net.TestUtil.Handlers.EventHandlers
{
    public class SimpleEventHandler : IEventHandler<SimpleEvent>
    {
        private readonly SimpleService _service;

        public SimpleEventHandler(SimpleService service)
        {
            _service = service;
        }
        public Task Handle(IReceiveContext<SimpleEvent> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
    
    public class SimpleEventThrowArgumentExceptionHandler : IEventHandler<SimpleEvent2>
    {
        public async Task Handle(IReceiveContext<SimpleEvent2> context, CancellationToken cancellationToken)
        {
            throw new ArgumentException("aaa");
        }
    }
}
