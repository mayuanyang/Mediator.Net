using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    public class TestBaseCommandHandlerThrowException : ICommandHandler<TestBaseCommand>
    {
        public Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            throw new NotImplementedException();
        }
    }
}
