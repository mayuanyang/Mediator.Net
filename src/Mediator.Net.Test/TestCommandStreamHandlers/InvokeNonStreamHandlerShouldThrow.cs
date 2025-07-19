using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandStreamHandlers
{
    public class InvokeNonStreamHandlerShouldThrow: TestBase
    {
        private IMediator _mediator;
        private Func<Task<TestCommandResponse>> _invokation;
        
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
            _invokation = () => _mediator.SendAsync<TestCommandWithResponse, TestCommandResponse>(new TestCommandWithResponse());
            
            return Task.CompletedTask;
        }

        Task ThenItShouldThrowNotSupportedException()
        {
            _invokation.ShouldThrow<NotSupportedException>(
                "Connecting to a IStreamRequestHandler should use the method of mediator.CreateStream");

            return Task.CompletedTask;
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}