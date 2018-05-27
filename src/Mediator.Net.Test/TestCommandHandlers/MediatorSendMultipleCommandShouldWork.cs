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
    
    public class MediatorSendMultipleCommandShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task1;
        private Task _task2;

        public MediatorSendMultipleCommandShouldWork()
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
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    new MessageBinding(typeof(DerivedTestBaseCommand), typeof(DerivedTestBaseCommandHandler))
                };
                return binding;
            }).Build();

        }

        void WhenTwoCommandsAreSent()
        {
            _task1 = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            _task2 = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));

        }

        void ThenItShouldReachTheRightHandler()
        {
            _task1.Wait();
            _task2.Wait();
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
