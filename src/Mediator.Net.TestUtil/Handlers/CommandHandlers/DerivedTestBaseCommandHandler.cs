using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class DerivedTestBaseCommandHandler: ICommandHandler<DerivedTestBaseCommand>
    {
    
        public Task Handle(IReceiveContext<DerivedTestBaseCommand> context, CancellationToken cancellationToken )
        {
            RubishBox.Rublish.Add(nameof(DerivedTestBaseCommandHandler));
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(10);
        }
    }
}
