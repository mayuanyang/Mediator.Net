using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.CommandHandlers
{
    class TestBaseCommandHandlerRaiseEvent : ICommandHandler<TestBaseCommand>
    {
        
        public async Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            RubishBox.Rublish.Add("TestBaseCommandHandlerRaiseEvent");
            Console.WriteLine($"Handling command {context.Message.Id}");
            await context.PublishAsync(new TestEvent(context.Message.Id));
           
        }
    }
}
