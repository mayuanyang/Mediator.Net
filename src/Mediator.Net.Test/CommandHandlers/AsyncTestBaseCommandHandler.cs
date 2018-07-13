using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class AsyncTestBaseCommandHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(nameof(AsyncTestBaseCommandHandler));
            await Task.FromResult(0);
        }
    }
}
