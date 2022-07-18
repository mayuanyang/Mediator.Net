using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers
{
    public class SimpleRequestHandler : IRequestHandler<SimpleRequest, SimpleResponse>
    {
        private readonly SimpleService _simpleService;

        public SimpleRequestHandler(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }

        public Task<SimpleResponse> Handle(IReceiveContext<SimpleRequest> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SimpleResponse(context.Message.Message));
        }

        
    }

    public class SimpleRequestWillThrowHandler : IRequestHandler<SimpleRequestWillThrow, SimpleResponse>
    {
        public Task<SimpleResponse> Handle(IReceiveContext<SimpleRequestWillThrow> context,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SimpleRequestThrowArgumentExceptionHandler : IRequestHandler<SimpleRequest, SimpleResponse>
    {
        public Task<SimpleResponse> Handle(IReceiveContext<SimpleRequest> context, CancellationToken cancellationToken)
        {
            throw new ArgumentException("cba");
        }
    }
}
