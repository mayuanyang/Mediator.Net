using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;

namespace Mediator.Net.TestUtil.Handlers
{
    public class SimpleCommandHandler : ICommandHandler<SimpleCommand>
    {
        private readonly SimpleService _simpleService;

        public SimpleCommandHandler(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }
        public Task Handle(ReceiveContext<SimpleCommand> context, CancellationToken cancellationToken)
        {
            _simpleService.DoWork();
            DummyTransaction transaction;
            if (context.TryGetService(out transaction))
            {
                transaction.Commit();
            }
            return Task.FromResult(0);
        }
    }
}
