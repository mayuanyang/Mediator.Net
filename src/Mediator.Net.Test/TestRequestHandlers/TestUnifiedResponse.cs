using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestRequestHandlers
{
    public class TestUnifiedResponse: TestBase
    {
        [Theory]
        [InlineData("RequestPipe")]
        [InlineData("GlobalPipe")]
        public async Task TestRequestWithUnifiedResult(string whichPipe)
        {
            var builder = SetupMediatorBuilderWithMiddleware(whichPipe);
            var mediator = SetupCommandMediatorWithUnifiedResultMiddleware(builder);
            
            var response = await 
                mediator.RequestAsync<GetGuidRequest, UnifiedResponse>(
                    new GetGuidRequest(Guid.Empty));
            
            response.Result.ShouldBeNull();
            response.Error.Code.ShouldBe(654321);
            response.Error.Message.ShouldBe("112233");
        }

        MediatorBuilder SetupMediatorBuilderWithMiddleware(string whichPipe)
        {
            var builder = new MediatorBuilder();
            
            switch (whichPipe)
            {
                case "RequestPipe":
                    builder.ConfigureRequestPipe(config => config.UseUnifyResultMiddleware(typeof(UnifiedResponse)));
                    break;
                case "GlobalPipe":
                    builder.ConfigureGlobalReceivePipe(config => config.UseUnifyResultMiddleware(typeof(UnifiedResponse)));
                    break;
            }

            return builder;
        }
        
        IMediator SetupCommandMediatorWithUnifiedResultMiddleware(MediatorBuilder builder)
        {
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(GetGuidRequest), typeof(UnifiedResponseRequesttHandler)),
                };
                
                return binding;
            }).Build();
        }
    }
}