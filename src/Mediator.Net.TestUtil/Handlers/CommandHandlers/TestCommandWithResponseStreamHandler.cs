using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TestCommandWithResponseStreamHandler : IStreamCommandHandler<TestCommandWithResponse, TestCommandResponse>
{
    public async IAsyncEnumerable<TestCommandResponse> Handle(IReceiveContext<TestCommandWithResponse> context, CancellationToken cancellationToken)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return await Task.FromResult(new TestCommandResponse() { Thing = i.ToString() });
        }
    }
}