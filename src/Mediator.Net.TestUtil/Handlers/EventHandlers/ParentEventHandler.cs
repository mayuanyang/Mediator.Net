using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.EventHandlers;

public class ParentEventHandler: IEventHandler<ParentEvent>
{
    public Task Handle(IReceiveContext<ParentEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.CompletedTask;
    }
}

public class ChildEventHandler : IEventHandler<ChildEvent>
{
    public Task Handle(IReceiveContext<ChildEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.CompletedTask;
    }
}

public class ParentAndChildEventCombinedHandler : IEventHandler<ParentEvent>, IEventHandler<ChildEvent>
{
    public Task Handle(IReceiveContext<ParentEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.CompletedTask;
    }

    public Task Handle(IReceiveContext<ChildEvent> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.CompletedTask;
    }
}