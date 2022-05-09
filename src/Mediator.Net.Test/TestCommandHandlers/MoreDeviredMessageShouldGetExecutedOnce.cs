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
    
    public class MoreDeviredMessageShouldGetExecutedOnce : TestBase
    {
        private IMediator _mediator;
        
        public MoreDeviredMessageShouldGetExecutedOnce()
        {
            ClearBinding();
        }
        void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    new MessageBinding(typeof(DerivedTestBaseCommand), typeof(DerivedTestBaseCommandHandler)),
                };
                return binding;
            }).Build();
            
        }

        async Task WhenAMoreDerivedCommandIsSent()
        {
            await _mediator.SendAsync(new DerivedTestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(1);
            RubishBox.Rublish.Contains(nameof(DerivedTestBaseCommandHandler)).ShouldBeTrue();

        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
