﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    public class GlobalPipe2MiddlewaresSecondOneThrowEx : TestBase
    {
        private IMediator _mediator;
        
        void GivenAMediatorWithGlobalPipeWith2Middlewares()
        {
            ClearBinding();
            
           var builder = new MediatorBuilder();
           
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent))
                    };
                    
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseConsoleLogger2();
                    x.UseMiddlewareThrowExBeforeConnect();
                })
                
            .Build();
        }

        async Task WhenACommandIsSent()
        {
            try
            {
                await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        void ThenBothMiddlewaresShouldHandleException()
        {
            RubishBox.Rublish.Where(x => x is Exception).ToList().Count.ShouldBe(2);
        }
        
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}