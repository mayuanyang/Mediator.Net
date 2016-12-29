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
    class OneCommandMultiHandleresShouldThrowException : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;
        public void GivenAMediatorBuilder()
        {
            _builder = new MediatorBuilder();
        }

        public void AndGivenABindingWithOneCommandMultipleHandlers()
        {
            _builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
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
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldThrowNoHandlerFoundException()
        {
            _task.ShouldThrow<MoreThanOneCommandHandlerException>();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
