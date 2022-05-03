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
using Xunit.Abstractions;

namespace Mediator.Net.Test.TestStreamRequestHandlers
{
    
    public class SendRequestShouldHandleException : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private IMediator _mediator;
        private IAsyncEnumerable<object> _result;
        private readonly Guid _guid = Guid.NewGuid();

        public SendRequestShouldHandleException(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
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
            try
            {
                await foreach (var r in _result)
                {
                    _testOutputHelper.WriteLine(counter.ToString());
                    counter++;
                }
            }
            catch (Exception e)
            {
                counter.ShouldBe(2);
                e.Message.ShouldBe("Exception after 2 response");
            }
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
