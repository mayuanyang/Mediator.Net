using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;

namespace Mediator.Net.Test.RequestHandlers
{
    class GetGuidRequestHandler2 : IRequestHandler<GetGuidRequest>
    {
        public Task<object> Handle(ReceiveContext<GetGuidRequest> context)
        {
            Console.WriteLine(context.Message.Id);
            object response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
