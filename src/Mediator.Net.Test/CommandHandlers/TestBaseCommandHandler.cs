using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class TestBaseCommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            await Console.Out.WriteLineAsync(context.Message.Id.ToString());
        }
    }
}
