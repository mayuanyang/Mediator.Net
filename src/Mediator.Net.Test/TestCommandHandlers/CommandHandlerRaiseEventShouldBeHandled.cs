using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class CommandHandlerRaiseEventShouldBeHandled : TestBase
    {
        private IMediator _mediator;

        public CommandHandlerRaiseEventShouldBeHandled()
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
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler))
                };
                
                return binding;
            })
            .ConfigurePublishPipe(x =>
                {
                    x.UseDummySave();
                })
            .Build();
        }

        async Task WhenACommandIsSent()
        {
           await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }
        
        void ThenItShouldPutSomeRubishIntoRublishBox()
        {
            RubishBox.Rublish.Contains(nameof(DummySave.UseDummySave)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandlerRaiseEvent)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}