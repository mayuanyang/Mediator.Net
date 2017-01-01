using System;
using System.Threading.Tasks;
using Mediator.Net.Autofac.Test.Messages;
using Mediator.Net.Autofac.Test.Services;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Autofac.Test.Handlers
{
    class SimpleEventHandler : IEventHandler<SimpleEvent>
    {
        private readonly SimpleService _service;

        public SimpleEventHandler(SimpleService service)
        {
            _service = service;
        }
        public Task Handle(IReceiveContext<SimpleEvent> context)
        {
            return Task.FromResult(0);
        }
    }
}
