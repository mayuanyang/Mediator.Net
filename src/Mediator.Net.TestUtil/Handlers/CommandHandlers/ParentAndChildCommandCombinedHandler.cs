using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class ParentAndChildCommandCombinedHandler : ICommandHandler<ParentCommand>, ICommandHandler<ChildCommand>
{
    public Task Handle(IReceiveContext<ParentCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.CompletedTask;
    }

    public Task Handle(IReceiveContext<ChildCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.WhenAll();
    }
}