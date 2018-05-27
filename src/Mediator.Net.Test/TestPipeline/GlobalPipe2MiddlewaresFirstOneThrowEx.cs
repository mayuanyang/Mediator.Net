﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class GlobalPipe2MiddlewaresFirstOneThrowEx : TestBase
    {
        private IMediator _mediator;
        
        void GivenAMediatorWithGlobalPipeWith2Middlewares()
        {
            ClearBinding();
           var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent))
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseMiddlewareThrowExBeforeConnect();
                    x.UseConsoleLogger2();
                })
                
            .Build();
        }

        void WhenACommandIsSent()
        {
            try
            {
                _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid())).Wait();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        void ThenTheFirstMiddlewareShouldHandleException()
        {
            RubishBox.Rublish.Where(x => x is Exception).ToList().Count.ShouldBe(1);
        }

    
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
