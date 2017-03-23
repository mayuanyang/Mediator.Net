using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class CommandHandlerRaiseEventShouldBeHandled : TestBase
    {
        private IMediator _mediator;
        
        public void GivenAMediator()
        {
            ClearBinding();
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

        public async Task WhenACommandIsSent()
        {
           await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

     
        public void ThenItShouldPutSomeRubishIntoRublishBox()
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
