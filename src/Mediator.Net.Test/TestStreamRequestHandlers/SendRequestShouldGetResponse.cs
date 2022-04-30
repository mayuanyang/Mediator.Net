using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestStreamRequestHandlers
{
    
    public class SendRequestShouldGetMultipleResponse : TestBase
    {
        private IMediator _mediator;
        private IAsyncEnumerable<object> _result;
        private readonly Guid _guid = Guid.NewGuid();
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetMultipleGuidStreamRequestHandler))
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

        Task WhenARequestIsSent()
        {
            _result = _mediator.CreateStream<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
            
            return Task.CompletedTask;
        }

        async Task ThenTheResultShouldBeReturn()
        {
            
            var counter = 0;
            for(var i = 0; i< 10; i++)
            {
                if (await _result.GetAsyncEnumerator().MoveNextAsync())
                    counter++;
            }
            
            counter.ShouldBe(10);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
