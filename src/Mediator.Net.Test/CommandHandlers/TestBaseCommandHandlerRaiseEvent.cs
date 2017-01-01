using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class TestBaseCommandHandlerRaiseEvent : ICommandHandler<TestBaseCommand>
    {
        
        public async Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            Console.WriteLine($"Handling command {context.Message.Id}");
            await context.PublishAsync(new TestEvent(context.Message.Id));
           
        }
    }
}
