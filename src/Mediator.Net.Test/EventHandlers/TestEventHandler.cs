using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.EventHandlers
{
    class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task Handle(IReceiveContext<TestEvent> context, CancellationToken cancellationToken = default(CancellationToken))
        {
            RubishBox.Rublish.Add(nameof(TestEventHandler));
            Console.WriteLine($"Hi, i am event {context.Message.Id}");
            return Task.FromResult(0);
        }
    }
}
