using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    
    public class AsyncCommandHandlerShouldWork : TestBase
    {

        private IMediator _mediator;

        public AsyncCommandHandlerShouldWork()
        {
            ClearBinding();
        }
        void GivenAMediator()
        {
            
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(AsyncTestBaseCommandHandler)) };
                return binding;
            })
            .Build();
        }

        async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Contains(nameof(AsyncTestBaseCommandHandler)).ShouldBe(true);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
