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
    
    public class NoHandlerForMessageShouldThrow : TestBase
    {
        private IMediator _mediator;
        private Task _task;

        public NoHandlerForMessageShouldThrow()
        {
            ClearBinding();
        }
        
        void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler))
                
                };
                
                return binding;
            }).Build();
           
        }

        void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldThrowNoHandlerFoundException()
        {
            Should.Throw<NoHandlerFoundException>(_task);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}