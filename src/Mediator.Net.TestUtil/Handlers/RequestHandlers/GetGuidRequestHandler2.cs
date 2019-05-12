using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers
{
    public class GetGuidRequestHandler2 : IRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public Task<GetGuidResponse> Handle(IReceiveContext<GetGuidRequest> context, CancellationToken cancellationToken)
        {
            Console.WriteLine(context.Message.Id);
            var response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
