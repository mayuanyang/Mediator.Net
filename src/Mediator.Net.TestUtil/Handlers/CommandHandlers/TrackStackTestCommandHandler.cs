using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TrackStackTestCommandHandler : ICommandHandler<TrackStackTestCommand>
{
    public Task Handle(IReceiveContext<TrackStackTestCommand> context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}