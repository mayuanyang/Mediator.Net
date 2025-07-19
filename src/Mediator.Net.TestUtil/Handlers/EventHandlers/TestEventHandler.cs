using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.EventHandlers;

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task Handle(IReceiveContext<TestEvent> context, CancellationToken cancellationToken)
    {
        if (context.Message.ShouldThrow)
            throw new BusinessException() { Error = "Error from event handler", Code = 50002 };
            
        TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        
        RubishBox.Rublish.Add(nameof(TestEventHandler));
            
        Console.WriteLine($"Hi, i am event {context.Message.Id}");
            
        return Task.FromResult(0);
    }
}

public class MultiTestEventHandlerHandleTheSameEvent : IEventHandler<TestEvent>
{
    public Task Handle(IReceiveContext<TestEvent> context, CancellationToken cancellationToken)
    {
        TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        RubishBox.Rublish.Add(nameof(MultiTestEventHandlerHandleTheSameEvent));
        
        Console.WriteLine($"Hi, i am event {context.Message.Id}");
        
        return Task.FromResult(0);
    }
}