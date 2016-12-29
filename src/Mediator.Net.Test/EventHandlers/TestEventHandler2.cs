using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.EventHandlers
{
    class TestEventHandler2 : IEventHandler<TestEvent>
    {
        public Task Handle(IReceiveContext<TestEvent> context)
        {
            Console.WriteLine($"Hi, i am number 2 event {context.Message.Id}");
            return Task.FromResult(0);
        }
    }
}
