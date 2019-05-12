using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class NoWorkCommandHandler : ICommandHandler<NoWorkCommand>
    {
        public Task Handle(IReceiveContext<NoWorkCommand> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
