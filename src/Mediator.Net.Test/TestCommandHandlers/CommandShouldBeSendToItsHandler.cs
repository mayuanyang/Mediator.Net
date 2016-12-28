using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using NUnit.Framework;
using TestStack.BDDfy;
using Moq;
using Shouldly;

namespace Mediator.Net.Test
{
    class CommandShouldBeSendToItsHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
          
            var binding = new List<MessageBinding>{new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler))};
            var builder = new MediatorBuilder();
            builder.RegisterHandlers(binding);
            var receivePipe =
                new ReceivePipe<IContext<IMessage>>(
                    new EmptyPipeSpecification<IContext<IMessage>>(), null);
            _mediator = new Mediator(receivePipe, null);
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
