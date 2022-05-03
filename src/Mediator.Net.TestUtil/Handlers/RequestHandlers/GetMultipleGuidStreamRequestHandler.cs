using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.RequestHandlers
{
    public class GetMultipleGuidStreamRequestHandler : IStreamRequestHandler<GetGuidRequest, GetGuidResponse>
    {
        public async IAsyncEnumerable<GetGuidResponse> Handle(IReceiveContext<GetGuidRequest> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
            RubishBox.Rublish.Add(nameof(GetGuidRequestHandler));

            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(100, cancellationToken);
                yield return await Task.FromResult(new GetGuidResponse(Guid.NewGuid() ){Index = i});
            }
                
            
        }
    }
    
    
}
