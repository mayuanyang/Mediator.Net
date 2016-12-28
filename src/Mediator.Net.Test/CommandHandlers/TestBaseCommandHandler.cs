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

        public Task Handle(ReceiveContext<TestBaseCommand> context)
        {
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(0);
        }
    }
}
