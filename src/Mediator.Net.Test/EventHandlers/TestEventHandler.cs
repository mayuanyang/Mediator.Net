using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.EventHandlers
{
    class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task Handle(IReceiveContext<TestEvent> context)
        {
            Console.WriteLine($"Hi, i am event {context.Message.Id}");
            return Task.FromResult(0);
        }
    }
}
