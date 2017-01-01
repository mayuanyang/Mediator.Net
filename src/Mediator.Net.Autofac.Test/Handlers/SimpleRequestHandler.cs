using System.Threading.Tasks;
using Mediator.Net.Autofac.Test.Messages;
using Mediator.Net.Autofac.Test.Services;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Autofac.Test.Handlers
{
    class SimpleRequestHandler : IRequestHandler<SimpleRequest, SimpleResponse>
    {
        private readonly SimpleService _simpleService;

        public SimpleRequestHandler(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }
        public Task<SimpleResponse> Handle(ReceiveContext<SimpleRequest> context)
        {
            return Task.FromResult(new SimpleResponse());
        }
    }
}
