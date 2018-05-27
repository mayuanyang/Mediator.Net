using Mediator.Net.Binding;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class ResponseShouldPropagateAcrossManyMiddlewares : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
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
