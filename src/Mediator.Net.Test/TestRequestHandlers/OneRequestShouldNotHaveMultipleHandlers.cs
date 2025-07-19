﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using Mediator.Net.TestUtil.Middlewares;

namespace Mediator.Net.Test.TestRequestHandlers
{
    public class OneRequestShouldNotHaveMultipleHandlers : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private readonly Guid _guid = Guid.NewGuid();
        
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            
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

        Task WhenARequestIsSent()
        {
            _task = _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
            
            return Task.FromResult(0);
        }

        void ThenTheResultShouldBeReturn()
        {
            _task.ShouldThrow<MoreThanOneHandlerException>();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}