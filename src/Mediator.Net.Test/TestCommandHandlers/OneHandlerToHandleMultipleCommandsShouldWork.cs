using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.IoCTestUtil.TestUtils;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    
    public class OneHandlerToHandleMultipleCommandsShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task1;
        private Task _task2;

        public OneHandlerToHandleMultipleCommandsShouldWork()
        {
            ClearBinding();
        }
        void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(AnotherCommand), typeof(MultiCommandsHandler)),
                    new MessageBinding(typeof(DerivedTestBaseCommand), typeof(MultiCommandsHandler))
                };
                return binding;
            }).Build();

        }

        async Task WhenTwoCommandsAreSent()
        {
            await _mediator.SendAsync(new AnotherCommand(Guid.NewGuid()));
            await _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(MultiCommandsHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
