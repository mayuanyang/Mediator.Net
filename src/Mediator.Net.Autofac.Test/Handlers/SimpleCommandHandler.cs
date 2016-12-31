using System.Threading.Tasks;
using Mediator.Net.Autofac.Test.Messages;
using Mediator.Net.Autofac.Test.Services;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Autofac.Test.Handlers
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
