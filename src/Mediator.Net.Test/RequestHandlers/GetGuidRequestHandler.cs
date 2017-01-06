using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;

namespace Mediator.Net.Test.RequestHandlers
{
    class GetGuidRequestHandler : IRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public Task<GetGuidResponse> Handle(ReceiveContext<GetGuidRequest> context)
        {
            RubishBox.Rublish.Add(nameof(GetGuidRequestHandler));
            Console.WriteLine(context.Message.Id);
            var response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
