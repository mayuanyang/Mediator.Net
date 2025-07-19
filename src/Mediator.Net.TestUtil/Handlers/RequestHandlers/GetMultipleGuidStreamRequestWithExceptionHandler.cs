using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers;

public class GetMultipleGuidStreamRequestWithExceptionHandler : IStreamRequestHandler<GetGuidRequest, GetGuidResponse>
{
    public async IAsyncEnumerable<GetGuidResponse> Handle(IReceiveContext<GetGuidRequest> context, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        RubishBox.Rublish.Add(nameof(GetGuidRequestHandler));

        yield return await Task.FromResult(new GetGuidResponse(Guid.NewGuid()){ Index = 0});
        yield return await Task.FromResult(new GetGuidResponse(Guid.NewGuid()){ Index = 1});
        
        throw new Exception("Exception after 2 response");
    }
}