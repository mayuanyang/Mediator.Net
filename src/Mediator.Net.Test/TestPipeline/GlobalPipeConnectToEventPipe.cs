﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;


namespace Mediator.Net.Test.TestPipeline
{
    
    public class GlobalPipeConnectToEventPipe : TestBase
    {
        private IMediator _mediator;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        void GivenAMediator()
        {
            ClearBinding();
           var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseConsoleLogger1();
                })
                .ConfigureEventReceivePipe(x =>
                {
                    x.UseConsoleLogger2();
                })
            .Build();


        }

        void WhenACommandIsSent()
        {
            _commandTask = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            
        }

        void ThenItShouldUseTheRightMiddlewares()
        {
            _commandTask.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

    
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
