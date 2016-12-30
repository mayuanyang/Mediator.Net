using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.RequestHandlers
{
    class GetGuidRequestHandler : IRequestHandler<GetGuidRequest>
    {
        public Task<object> Handle(ReceiveContext<GetGuidRequest> context)
        {
            object response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
