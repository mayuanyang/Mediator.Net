using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers;

public class MultiRequestsHandler: IRequestHandler<GetGuidRequest, GetGuidResponse>, IRequestHandler<SimpleRequest, SimpleResponse>
{
    public Task<GetGuidResponse> Handle(IReceiveContext<GetGuidRequest> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.FromResult(new GetGuidResponse(context.Message.Id));
    }

    public Task<SimpleResponse> Handle(IReceiveContext<SimpleRequest> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Message);
        
        return Task.FromResult(new SimpleResponse(""));
    }
}