using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.RequestHandlers
{
    class GetGuidRequestHandler : IRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public Task<GetGuidResponse> Handle(ReceiveContext<GetGuidRequest> context)
        {
            var response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
