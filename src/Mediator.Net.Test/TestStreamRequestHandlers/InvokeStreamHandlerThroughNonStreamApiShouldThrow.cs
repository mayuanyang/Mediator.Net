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
    
    public class InvokeStreamHandlerThroughNonStreamApiShouldThrow : TestBase
    {
        private IMediator _mediator;
        private Func<Task<GetGuidResponse>> _invokation;
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

        Task WhenARequestIsSentThroughNonStreamApi()
        {
            _invokation = () => _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
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
