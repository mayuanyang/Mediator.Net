using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Sample.Web.Commands;
using Mediator.Net.Sample.Web.Services;

namespace Mediator.Net.Sample.Web.CommandHandlers
{
    public class SimpleCommandHandler : ICommandHandler<SimpleCommand>
    {
        private readonly ISimpleService _service;

        public SimpleCommandHandler(ISimpleService service)
        {
            _service = service;
        }
        public Task Handle(ReceiveContext<SimpleCommand> context)
        {
            _service.DoWork();
            Console.WriteLine(context.Message.Id);
            return Task.FromResult(0);
        }
    }
}