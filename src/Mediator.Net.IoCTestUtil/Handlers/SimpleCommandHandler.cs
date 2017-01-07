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
        public Task Handle(ReceiveContext<SimpleCommand> context)
        {
            _simpleService.DoWork();
            return Task.FromResult(0);
        }
    }
}
