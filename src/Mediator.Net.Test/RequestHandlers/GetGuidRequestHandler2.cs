using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.RequestHandlers
{
    class GetGuidRequestHandler2 : IRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public Task<GetGuidResponse> Handle(ReceiveContext<GetGuidRequest> context, CancellationToken cancellationToken)
        {
            Console.WriteLine(context.Message.Id);
            var response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
