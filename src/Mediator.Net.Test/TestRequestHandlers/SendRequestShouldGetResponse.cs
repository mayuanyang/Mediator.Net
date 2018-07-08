using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestRequestHandlers
{
    
    public class SendRequestShouldGetResponse : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        private readonly Guid _guid = Guid.NewGuid();
        void GivenAMediatorAndTwoMiddlewares()
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
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseConsoleLogger1();
                    x.UseConsoleLogger2();
                })
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger3();
                })
            .Build();


        }

        async Task WhenARequestIsSent()
        {
            _result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
        }

        void ThenTheResultShouldBeReturn()
        {
            _result.Id.ShouldBe(_guid);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
