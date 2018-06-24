using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.CommandHandlers
{
    class DerivedTestBaseCommandHandler: ICommandHandler<DerivedTestBaseCommand>
    {
    
        public Task Handle(ReceiveContext<DerivedTestBaseCommand> context, CancellationToken cancellationToken = default(CancellationToken))
        {
            RubishBox.Rublish.Add(nameof(DerivedTestBaseCommandHandler));
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(10);
        }
    }
}
