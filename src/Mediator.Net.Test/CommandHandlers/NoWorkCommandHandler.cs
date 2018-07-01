using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    public class NoWorkCommandHandler : ICommandHandler<NoWorkCommand>
    {
        public Task Handle(ReceiveContext<NoWorkCommand> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
