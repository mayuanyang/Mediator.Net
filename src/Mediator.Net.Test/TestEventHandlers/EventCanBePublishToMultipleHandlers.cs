using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestEventHandlers
{
    class EventCanBePublishToMultipleHandlers : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {

            var builder = new MediatorBuilder();
            builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler2)),
                };
                return binding;
            });
            var receivePipe =
                new ReceivePipe<IContext<IMessage>>(
                    new EmptyPipeSpecification<IContext<IMessage>>(), null);
            _mediator = new Mediator(receivePipe, null);
        }

        public async Task WhenAEventIsSent()
        {
            _task = _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            await _task;
        }

        public void ThenItShouldReachTheRightHandlers()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
