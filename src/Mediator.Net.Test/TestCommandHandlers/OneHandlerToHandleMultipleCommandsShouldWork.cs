using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class OneHandlerToHandleMultipleCommandsShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task1;
        private Task _task2;
        public void GivenAMediator()
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

        public void WhenTwoCommandsAreSent()
        {
            _task1 = _mediator.SendAsync(new AnotherCommand(Guid.NewGuid()));
            _task2 = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));

        }

        public async Task ThenItShouldReachTheRightHandler()
        {
            await _task1;
            await _task2;
            _task1.Status.ShouldBe(TaskStatus.RanToCompletion);
            _task2.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(MultiCommandsHandler)).ShouldBeTrue();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
