using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers
{
    public class AsyncRequestHandler : IRequestHandler<GetGuidInAsyncRequest, GetGuidResponse>
    {
        public async Task<GetGuidResponse> Handle(ReceiveContext<GetGuidInAsyncRequest> context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return new GetGuidResponse(Guid.NewGuid());
        }
    }
}
