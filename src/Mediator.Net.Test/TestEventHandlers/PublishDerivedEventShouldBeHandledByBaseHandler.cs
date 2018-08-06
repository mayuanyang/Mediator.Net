using System;
using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    
    public class PublishDerivedEventShouldBeHandledByBaseHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;
        void GivenAMediatorBuilder()
        {
            ClearBinding();
            _builder = new MediatorBuilder();
            
        }

        void AndGivenTheEventIsRegisteredToItsBaseClassHandler()
        {
            _mediator = _builder.RegisterUnduplicatedHandlers().Build();
        }

        async Task WhenAMoreDerivedEventIsPublished()
        {
            await _mediator.PublishAsync(new DerivedEvent(Guid.NewGuid(), "ddd"));
        }

        void ThenItShouldReachTheBaseEventHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(DerivedEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(MultiTestEventHandlerHandleTheSameEvent)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
