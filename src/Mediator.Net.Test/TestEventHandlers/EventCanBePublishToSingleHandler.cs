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
    [Collection("Avoid parallel execution")]
    public class EventCanBePublishToSingleHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)) };
                return binding;
            }).Build();
            
        }

        public async Task WhenAEventIsPublished()
        {
            _task = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            await _task;

        }

        public void ThenItShouldReachTheRightHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
