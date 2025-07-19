using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public abstract class InheritanceBaseCommandHandler : ICommandHandler<InheritanceCommand>
{
    public abstract Task DoWork(string thing);
    
    public async Task Handle(IReceiveContext<InheritanceCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);

        await DoWork("From parent");
    }
}
