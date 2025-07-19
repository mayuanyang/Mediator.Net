using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TestCommandWithResponseHandler : ICommandHandler<TestCommandWithResponse, TestCommandResponse>
{
    public Task<TestCommandResponse> Handle(IReceiveContext<TestCommandWithResponse> context, CancellationToken cancellationToken)
    {
        var response = new TestCommandResponse() { Thing = "Hello world" };
            
        return Task.FromResult(response);
    }
}
    
public class TestCommandWithResponseThatThrowHandler : ICommandHandler<TestCommandWithResponse, TestCommandResponse>
{
    public Task<TestCommandResponse> Handle(IReceiveContext<TestCommandWithResponse> context, CancellationToken cancellationToken)
    {
        throw new ArgumentException("abc");
    }
}