using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class AsyncTestBaseCommandHandler : ICommandHandler<TestBaseCommand>
{
    public async Task Handle(IReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(nameof(AsyncTestBaseCommandHandler));
            
        await Task.FromResult(0);
    }
}
