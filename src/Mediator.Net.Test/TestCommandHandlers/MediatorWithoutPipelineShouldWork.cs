using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    
    public class MediatorWithoutPipelineShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task;

        public MediatorWithoutPipelineShouldWork()
        {
            ClearBinding();
        }
        void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(AsyncTestBaseCommandHandler)) };
                return binding;
            }).Build();
        }

        Task WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            return _task;
        }

        void ThenItShouldReachTheRightHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Contains(nameof(AsyncTestBaseCommandHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
