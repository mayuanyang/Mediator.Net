using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestPipeline
{
    class GlobalPipeConnectToEventPipe : TestBase
    {
        private IMediator _mediator;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        public void GivenAMediator()
        {
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

        public void WhenACommandIsSent()
        {
            _commandTask = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            
        }

        public void ThenItShouldUseTheRightMiddlewares()
        {
            _commandTask.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

    
        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
