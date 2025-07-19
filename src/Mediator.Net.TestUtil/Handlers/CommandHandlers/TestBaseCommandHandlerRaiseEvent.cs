using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class TestBaseCommandHandlerRaiseEvent : ICommandHandler<TestBaseCommand>
{
    public async Task Handle(IReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
    {
        TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        RubishBox.Rublish.Add("TestBaseCommandHandlerRaiseEvent");
        
        Console.WriteLine($"Handling command { context.Message.Id }");
        
        await context.PublishAsync(new TestEvent(context.Message.Id), cancellationToken);
    }
}