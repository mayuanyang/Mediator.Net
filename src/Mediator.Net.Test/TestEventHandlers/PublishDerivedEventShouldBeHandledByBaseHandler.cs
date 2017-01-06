using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestEventHandlers
{
    class PublishDerivedEventShouldBeHandledByBaseHandler
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;
        public void GivenAMediatorBuilder()
        {
            _builder = new MediatorBuilder();
            
        }

        public void AndGivenTheEventIsRegisteredToItsBaseClassHandler()
        {
            _mediator = _builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(DerivedEvent), typeof(TestEventHandler)),
                    new MessageBinding(typeof(DerivedEvent), typeof(DerivedEventHandler))
                };
                return binding;
            }).Build();
        }

        public async Task WhenAMoreDerivedEventIsPublished()
        {
            _task = _mediator.PublishAsync(new DerivedEvent(Guid.NewGuid(), "ddd"));
            await _task;

        }

        public void ThenItShouldReachTheBaseEventHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(DerivedEventHandler)).ShouldBeTrue();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
