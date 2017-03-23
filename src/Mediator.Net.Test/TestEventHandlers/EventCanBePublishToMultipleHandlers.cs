using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    public class EventCanBePublishToMultipleHandlers : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler2)),
                };
                return binding;
            })
            .Build();
            
        }

        public async Task WhenAEventIsSent()
        {
            _task = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            await _task;
        }

        public void ThenItShouldReachTheRightHandlers()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler2)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
