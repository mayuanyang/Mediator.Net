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
    public class NoHandlerForMessageShouldThrow : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                
                };
                return binding;
            }).Build();
           
        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldThrowNoHandlerFoundException()
        {
            _task.ShouldThrow<NoHandlerFoundException>();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
