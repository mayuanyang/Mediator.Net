using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    class MoreDeviredMessageShouldGetExecutedOnce : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
          
            var binding = new Dictionary<Type, Type>
            {
                {typeof(TestBaseCommand), typeof(TestBaseCommandHandler)},
                {typeof(DerivedTestBaseCommand), typeof(DerivedTestBaseCommandHandler)},
                
                
            };
            var builder = new MediatorBuilder();
            builder.RegisterHandlers(binding);
            var receivePipe =
                new ReceivePipe<IContext<IMessage>>(
                    new EmptyPipeSpecification<IContext<IMessage>>(), null);
            _mediator = new Mediator(receivePipe, null);
        }

        public void WhenACommandIsSent()
        {
             _task = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
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
