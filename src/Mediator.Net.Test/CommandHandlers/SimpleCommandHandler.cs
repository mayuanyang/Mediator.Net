using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.CommandHandlers
{
    class SimpleCommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var value = context.MetaData.ContainsKey("something");
            RubishBox.Rublish.Add(value);
            
            await Task.FromResult(0);
        }
    }
}
