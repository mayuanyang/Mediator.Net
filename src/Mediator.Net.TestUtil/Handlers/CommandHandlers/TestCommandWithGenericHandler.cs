using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TestCommandWithGenericHandler : ICommandHandler<TestCommandWithResponse, GenericUnifiedResponse<string>>
{
    public async Task<GenericUnifiedResponse<string>> Handle(IReceiveContext<TestCommandWithResponse> context, CancellationToken cancellationToken)
    {
        if (context.Message.ShouldThrow)
            throw new BusinessException
            {
                Code = 12345,
                Error = "An error has occured"
            };

        var result = new GenericUnifiedResponse<string> { Result = context.Message.Request + "Result" };

        if (context.Message.ShouldPublishEvent)
            await context.PublishAsync(new TestEvent(Guid.NewGuid()) { ShouldThrow = context.Message.ShouldEventHandlerThrow }, cancellationToken);

        return result;
    }
}