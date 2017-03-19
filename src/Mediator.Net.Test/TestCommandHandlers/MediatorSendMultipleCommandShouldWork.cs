using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class MediatorSendMultipleCommandShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task1;
        private Task _task2;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    new MessageBinding(typeof(DerivedTestBaseCommand), typeof(DerivedTestBaseCommandHandler))
                };
                return binding;
            }).Build();

        }

        public void WhenTwoCommandsAreSent()
        {
            _task1 = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            _task2 = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));

        }

        public async Task ThenItShouldReachTheRightHandler()
        {
            await _task1;
            await _task2;
            _task1.Status.ShouldBe(TaskStatus.RanToCompletion);
            _task2.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(DerivedTestBaseCommandHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
