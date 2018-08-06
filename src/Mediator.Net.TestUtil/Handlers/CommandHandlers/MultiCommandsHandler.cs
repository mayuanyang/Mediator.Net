using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class MultiCommandsHandler : ICommandHandler<AnotherCommand>, ICommandHandler<DerivedTestBaseCommand>
    {
        public Task Handle(ReceiveContext<AnotherCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);
            return Task.FromResult(1);
        }

        public async Task Handle(ReceiveContext<DerivedTestBaseCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);
            await Task.FromResult(1);
        }
    }
}
