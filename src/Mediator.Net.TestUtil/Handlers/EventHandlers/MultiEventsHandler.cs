using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.EventHandlers;

public class MultiEventsHandler: IEventHandler<SimpleEvent>, IEventHandler<TestEvent>
{
    public Task Handle(IReceiveContext<SimpleEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.FromResult(0);
    }

    public Task Handle(IReceiveContext<TestEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.FromResult(0);
    }
}