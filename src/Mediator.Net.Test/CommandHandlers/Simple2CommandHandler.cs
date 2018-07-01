using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.CommandHandlers
{
    class Simple2CommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            var userName = context.MetaData["UserName"];
            RubishBox.Rublish.Add(userName);

            var password = context.MetaData["Password"];
            RubishBox.Rublish.Add(password);

            await Task.FromResult(0);
        }
    }
}
