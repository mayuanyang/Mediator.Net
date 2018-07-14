using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class TestBaseCommandHandler : ICommandHandler<TestBaseCommand>
    {
        
        public Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            Console.WriteLine(context.Message.Id);
            RubishBox.Rublish.Add(nameof(TestBaseCommandHandler));
            return Task.FromResult(0);
        }
    }
}
