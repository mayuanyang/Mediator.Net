using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    [Collection("Avoid parallel execution")]
    public class OneCommandMultiHandleresShouldThrowException : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;

        public OneCommandMultiHandleresShouldThrowException()
        {
            ClearBinding();
        }
        public void GivenAMediatorBuilder()
        {
            _builder = new MediatorBuilder();
        }

        public void AndGivenABindingWithOneCommandMultipleHandlers()
        {
            _mediator = _builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),

                };
                return binding;
            }).Build();
            
        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldThrowNoHandlerFoundException()
        {
            _task.ShouldThrow<MoreThanOneHandlerException>();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
