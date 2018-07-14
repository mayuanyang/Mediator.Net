using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class TestBaseCommandHandlerThrowException : ICommandHandler<TestBaseCommand>
    {
        public Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
