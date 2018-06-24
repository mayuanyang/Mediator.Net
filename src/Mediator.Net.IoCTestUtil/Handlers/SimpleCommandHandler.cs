using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Services;

namespace Mediator.Net.IoCTestUtil.Handlers
{
    class SimpleCommandHandler : ICommandHandler<SimpleCommand>
    {
        private readonly SimpleService _simpleService;

        public SimpleCommandHandler(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }
        public Task Handle(ReceiveContext<SimpleCommand> context, CancellationToken cancellationToken = default(CancellationToken))
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
