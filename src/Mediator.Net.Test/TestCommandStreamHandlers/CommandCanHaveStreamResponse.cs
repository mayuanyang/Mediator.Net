using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using Xunit.Abstractions;

namespace Mediator.Net.Test.TestCommandStreamHandlers
{
    public class CommandCanHaveStreamResponse: TestBase
    {
        private IMediator _mediator;
        private IAsyncEnumerable<TestCommandResponse> _result;
        
        
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestCommandWithResponse), typeof(TestCommandWithResponseStreamHandler)),
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

        Task WhenACommandIsSent()
        {
            _result = _mediator.CreateStream<TestCommandWithResponse, TestCommandResponse>(new TestCommandWithResponse());
            
            return Task.CompletedTask;
        }

        async Task ThenTheResultShouldBeReturn()
        {
            var counter = 0;
            await foreach (var r in _result)
            {
                r.Thing.ShouldBe(counter.ToString());
                counter++;
            }
            
            counter.ShouldBe(5);
            
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}