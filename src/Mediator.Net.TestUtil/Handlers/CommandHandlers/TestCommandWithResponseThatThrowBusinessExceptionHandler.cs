using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TestCommandWithResponseThatThrowBusinessExceptionHandler : ICommandHandler<TestCommandWithResponse, UnifiedResponse>
{
    public Task<UnifiedResponse> Handle(IReceiveContext<TestCommandWithResponse> context, CancellationToken cancellationToken)
    {
        if (context.Message.ShouldThrow)
        {
            throw new BusinessException()
            {
                Code = 12345,
                Error = "An error has occured"
            };    
        }

        var result = new UnifiedResponse { Result = context.Message.Request + "Result" };
        
        return Task.FromResult(result);
    }
}