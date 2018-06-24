using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.EventHandlers
{
    class TestEventHandler2 : IEventHandler<TestEvent>
    {
        public Task Handle(IReceiveContext<TestEvent> context, CancellationToken cancellationToken = default(CancellationToken))
        {
            RubishBox.Rublish.Add(nameof(TestEventHandler2));
            Console.WriteLine($"Hi, i am number 2 event {context.Message.Id}");
            return Task.FromResult(0);
        }
    }
}
