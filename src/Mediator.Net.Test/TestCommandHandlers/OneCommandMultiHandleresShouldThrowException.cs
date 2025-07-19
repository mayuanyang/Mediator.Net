using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class OneCommandMultiHandleresShouldThrowException : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        private MediatorBuilder _builder;

        public OneCommandMultiHandleresShouldThrowException()
        {
            ClearBinding();
        }
        
        void GivenAMediatorBuilder()
        {
            _builder = new MediatorBuilder();
        }

        void AndGivenABindingWithOneCommandMultipleHandlers()
        {
            _mediator = _builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler))
                };
                return binding;
            }).Build();
            
        }

        void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldThrowNoHandlerFoundException()
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