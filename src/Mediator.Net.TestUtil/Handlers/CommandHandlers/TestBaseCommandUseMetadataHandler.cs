using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public class TestBaseCommandUseMetadataHandler : ICommandHandler<TestBaseCommand>
    {
        public async Task Handle(ReceiveContext<TestBaseCommand> context, CancellationToken cancellationToken)
        {
            var userName = context.MetaData["UserName"];
            RubishBox.Rublish.Add(userName);

            var password = context.MetaData["Password"];
            RubishBox.Rublish.Add(password);

            await Task.FromResult(0);
        }
    }
}
