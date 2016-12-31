using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class MultiCommandsHandler : ICommandHandler<AnotherCommand>, ICommandHandler<DerivedTestBaseCommand>
    {
        public Task Handle(ReceiveContext<AnotherCommand> context)
        {
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(1);
        }

        public async Task Handle(ReceiveContext<DerivedTestBaseCommand> context)
        {
            Console.WriteLine(context.Message.Id);
            await Task.FromResult(1);
        }
    }
}
