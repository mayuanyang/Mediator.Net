using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.EventHandlers
{
    class DerivedEventHandler : IEventHandler<DerivedEvent>
    {
        public Task Handle(IReceiveContext<DerivedEvent> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(nameof(DerivedEventHandler));
            return Task.FromResult(1);
        }
    }
}
