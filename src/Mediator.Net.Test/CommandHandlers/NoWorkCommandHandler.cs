using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    public class NoWorkCommandHandler : ICommandHandler<NoWorkCommand>
    {
        public Task Handle(ReceiveContext<NoWorkCommand> context)
        {
            return Task.FromResult(0);
        }
    }
}
