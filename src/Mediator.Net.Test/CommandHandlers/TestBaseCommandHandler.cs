using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class TestBaseCommandHandler : ICommandHandler<TestBaseCommand>
    {
        private readonly Guid _id;

        public TestBaseCommandHandler()
        {
            
        }
        public TestBaseCommandHandler(Guid id)
        {
            _id = id;
        }
        public async Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            await Console.Out.WriteLineAsync(context.Message.Id.ToString());

           
        }
    }
}
