using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using Xunit.Abstractions;

namespace Mediator.Net.Test.TestStreamRequestHandlers
{
    
    public class SendRequestShouldHandleException : TestBase
    {
        private IMediator _mediator;
        private IAsyncEnumerable<GetGuidResponse> _result;
        private readonly Guid _guid = Guid.NewGuid();

        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetMultipleGuidStreamRequestWithExceptionHandler))
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseConsoleLogger3();
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
            try
            {
                await foreach (var r in _result)
                {
                    r.Index.ShouldBe(counter);
                    counter++;
                }
            }
            catch (Exception e)
            {
                RubishBox.Rublish.Count.ShouldBe(3);
                counter.ShouldBe(2);
                e.Message.ShouldBe("Exception after 2 response");
            }
            
        }

        Task AndThenTheMiddlewareShouldOnlyRunOnce()
        {
            TokenRecorder.Recorder.Count.ShouldBe(7);
            RubishBox.Rublish.Count.ShouldBe(3);
            return Task.CompletedTask;
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
