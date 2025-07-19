using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers;

public abstract class InheritanceBaseRequestHandler : IRequestHandler<InheritanceRequest, InheritanceResponse>
{
    public abstract Task<InheritanceResponse> DoWork(InheritanceRequest request);
        
    public async Task<InheritanceResponse> Handle(IReceiveContext<InheritanceRequest> context, CancellationToken cancellationToken)
    {
        return await DoWork(context.Message);
    }
}

public class ChildRequestHandler : InheritanceBaseRequestHandler
{
    public override async Task<InheritanceResponse> DoWork(InheritanceRequest request)
    {
        var response = new InheritanceResponse(request.Id)
        {
            Content = "Hello world"
        };
        
        await Task.WhenAll();
        
        return response;
    }
}