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
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class NoHandlerForMessageShouldThrow : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
          
            
            var builder = new MediatorBuilder();
            builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                
                };
                return binding;
            });
            var receivePipe =
                new ReceivePipe<IContext<IMessage>>(
                    new EmptyPipeSpecification<IContext<IMessage>>(), null);
            _mediator = new Mediator(receivePipe, null);
        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldThrowNoHandlerFoundException()
        {
            _task.ShouldThrow<NoHandlerFoundException>();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
