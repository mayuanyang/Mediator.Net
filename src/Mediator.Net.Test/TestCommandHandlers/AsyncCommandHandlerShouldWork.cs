using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class AsyncCommandHandlerShouldWork : TestBase
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
            })
            .Build();
           
        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
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
