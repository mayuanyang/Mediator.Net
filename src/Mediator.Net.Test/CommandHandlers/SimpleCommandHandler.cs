using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class SimpleCommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            var value = context.MetaData.ContainsKey("something");
            RubishBox.Rublish.Add(value);
            
            await Task.FromResult(0);
        }
    }
}
