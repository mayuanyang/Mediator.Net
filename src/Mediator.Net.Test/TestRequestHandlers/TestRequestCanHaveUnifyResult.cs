using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestRequestHandlers
{
    public class TestRequestCanHaveUnifyResult: TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        Task GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x => x.UseChangeRequestResultMiddleware())
                .Build();
            return Task.CompletedTask;
        }

        async Task WhenARequestIsSent()
        {
            _result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(Guid.NewGuid()));
        }

        Task ThenTheResultShouldBeReturn()
        {
            _result.ToBeSetByMiddleware.ShouldBe("i am from middleware");
            return Task.CompletedTask;
        }
        
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}