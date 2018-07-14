using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;

namespace Mediator.Net.TestUtil.Handlers
{
    public class SimpleRequestHandler : IRequestHandler<SimpleRequest, SimpleResponse>
    {
        private readonly SimpleService _simpleService;

        public SimpleRequestHandler(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }
        public Task<SimpleResponse> Handle(ReceiveContext<SimpleRequest> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SimpleResponse(context.Message.Message));
        }
    }
}
