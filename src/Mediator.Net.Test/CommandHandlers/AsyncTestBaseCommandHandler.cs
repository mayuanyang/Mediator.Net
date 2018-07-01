using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.CommandHandlers
{
    class AsyncTestBaseCommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(nameof(AsyncTestBaseCommandHandler));
            Console.WriteLine(context.Message.Id);
            await Task.FromResult(0);
        }
    }
}
