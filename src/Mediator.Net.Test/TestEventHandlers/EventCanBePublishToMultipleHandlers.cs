using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    public class EventCanBePublishToMultipleHandlers : TestBase
    {
        private IMediator _mediator;
        
        void GivenAMediator()
        {
            ClearBinding();
            
            var builder = new MediatorBuilder();
            
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                    new MessageBinding(typeof(TestEvent), typeof(MultiTestEventHandlerHandleTheSameEvent)),
                };
                
                return binding;
            })
            .Build();
        }

        async Task WhenAEventIsSent()
        {
            await _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandlers()
        {
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(MultiTestEventHandlerHandleTheSameEvent)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}