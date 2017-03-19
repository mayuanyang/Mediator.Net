using System;
using System.Threading.Tasks;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    class PublishDerivedEventShouldBeHandledByBaseHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;
        public void GivenAMediatorBuilder()
        {
            ClearBinding();
            _builder = new MediatorBuilder();
            
        }

        public void AndGivenTheEventIsRegisteredToItsBaseClassHandler()
        {
            _mediator = _builder.RegisterHandlers(typeof(PublishDerivedEventShouldBeHandledByBaseHandler).Assembly()).Build();
        }

        public async Task WhenAMoreDerivedEventIsPublished()
        {
            _task = _mediator.PublishAsync(new DerivedEvent(Guid.NewGuid(), "ddd"));
            await _task;

        }

        public void ThenItShouldReachTheBaseEventHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(DerivedEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler2)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
