using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.CommandHandlers
{
    class DerivedTestBaseCommandHandler: ICommandHandler<DerivedTestBaseCommand>
    {
    
        public Task Handle(ReceiveContext<DerivedTestBaseCommand> context, CancellationToken cancellationToken )
        {
            RubishBox.Rublish.Add(nameof(DerivedTestBaseCommandHandler));
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(10);
        }
    }
}
