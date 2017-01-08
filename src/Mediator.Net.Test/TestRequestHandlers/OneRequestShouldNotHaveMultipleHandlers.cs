using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestRequestHandlers
{
    class OneRequestShouldNotHaveMultipleHandlers
    {
        private IMediator _mediator;
        private Task _task;
        private readonly Guid _guid = Guid.NewGuid();
        public void GivenAMediatorAndTwoMiddlewares()
        {
            
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler)),
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler2))
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

        public Task WhenARequestIsSent()
        {
            _task = _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
            return Task.FromResult(0);
        }

        public void ThenTheResultShouldBeReturn()
        {
            _task.ShouldThrow<MoreThanOneHandlerException>();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
