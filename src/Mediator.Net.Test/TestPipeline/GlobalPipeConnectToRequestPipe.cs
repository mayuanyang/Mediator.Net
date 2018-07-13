using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class GlobalPipeConnectToRequestPipe : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _response;
        private Guid _id = Guid.NewGuid();
        void GivenAMediator()
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
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseConsoleLogger1();
                })
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger3();
                })
            .Build();


        }

        async Task WhenARequestIsSent()
        {
            _response = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_id));
        }

        void ThenTheRequestShouldBeHandled()
        {
            _response.Id.ShouldBe(_id);
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog3.UseConsoleLogger3)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(GetGuidRequestHandler)).ShouldBeTrue();
        }

    
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
