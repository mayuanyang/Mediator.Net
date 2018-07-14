using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.EventHandlers
{
    public class DerivedEventHandler : IEventHandler<DerivedEvent>
    {
        public Task Handle(IReceiveContext<DerivedEvent> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(nameof(DerivedEventHandler));
            return Task.FromResult(1);
        }
    }
}
