using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.EventHandlers
{
    class DerivedEventHandler : IEventHandler<DerivedEvent>
    {
        public Task Handle(IReceiveContext<DerivedEvent> context)
        {
            return Task.FromResult(1);
        }
    }
}
