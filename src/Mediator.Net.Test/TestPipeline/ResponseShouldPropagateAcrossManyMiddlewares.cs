using Mediator.Net.Binding;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class ResponseShouldPropagateAcrossManyMiddlewares : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        private readonly Guid _id = Guid.NewGuid();
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder
                .RegisterHandlers(() => new List<MessageBinding>()
                {
                    new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
                })
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger1();
                    x.UseConsoleLogger2();
                })
                .Build();
        }

        async Task WhenARequestIsSent()
        {
            _result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_id));
        }

        void ThenTheResponseShouldBeReceived()
        {
            _result.Id.ShouldBe(_id);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
