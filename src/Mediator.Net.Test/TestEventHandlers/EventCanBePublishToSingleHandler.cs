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
    
    public class EventCanBePublishToSingleHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)) };
                return binding;
            }).Build();
            
        }

        async Task WhenAEventIsPublished()
        {
            await _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
