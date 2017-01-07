using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Services;

namespace Mediator.Net.IoCTestUtil.Handlers
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
