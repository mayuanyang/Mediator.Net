using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public abstract class InheritanceBaseCommandHandler : ICommandHandler<SimpleCommand>
    {
        public abstract Task DoWork(string thing);
        public async Task Handle(IReceiveContext<SimpleCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);

            await DoWork("From parent");
        }
    }

    public class ChildCommandHandler : InheritanceBaseCommandHandler
    {
        public override async Task DoWork(string thing)
        {
            RubishBox.Rublish.Add(thing);
            await Task.WhenAll();
        }
    }

}
