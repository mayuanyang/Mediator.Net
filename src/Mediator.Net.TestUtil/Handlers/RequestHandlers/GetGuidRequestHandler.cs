using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers
{
    public class GetGuidRequestHandler : IRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public Task<GetGuidResponse> Handle(ReceiveContext<GetGuidRequest> context, CancellationToken cancellationToken)
        {
            TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
            RubishBox.Rublish.Add(nameof(GetGuidRequestHandler));
            Console.WriteLine(context.Message.Id);
            var response = new GetGuidResponse(context.Message.Id);
            return Task.FromResult(response);
        }
    }
}
