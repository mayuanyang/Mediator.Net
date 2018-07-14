using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;


namespace Mediator.Net.Test.TestPipeline
{

    public class GlobalPipeConnectToEventPipe : TestBase
    {
        private IMediator _mediator;
        private Task _eventTask;
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

        Task WhenACommandIsSent()
        {
            _eventTask = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            return _eventTask;
        }

        void ThenItShouldUseTheRightMiddlewares()
        {
            _eventTask.Status.ShouldBe(TaskStatus.RanToCompletion);
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
