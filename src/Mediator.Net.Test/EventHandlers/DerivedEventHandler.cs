using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.EventHandlers
{
    class DerivedEventHandler : IEventHandler<DerivedEvent>
    {
        public Task Handle(IReceiveContext<DerivedEvent> context)
        {
            RubishBox.Rublish.Add(nameof(DerivedEventHandler));
            return Task.FromResult(1);
        }
    }
}
