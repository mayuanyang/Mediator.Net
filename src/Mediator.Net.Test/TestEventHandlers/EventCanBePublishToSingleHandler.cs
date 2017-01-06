using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestEventHandlers
{
    class EventCanBePublishToSingleHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {

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

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
