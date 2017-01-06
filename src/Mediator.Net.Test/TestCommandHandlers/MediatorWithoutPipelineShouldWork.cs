using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class MediatorWithoutPipelineShouldWork : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
                     
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(AsyncTestBaseCommandHandler)) };
                return binding;
            }).Build();
            
        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Contains(nameof(AsyncTestBaseCommandHandler)).ShouldBeTrue();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
