using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    public class TestParentAndChildCombinedHandler
    {
        [Fact]
        public async Task TestCombinedEventForChild()
        {
            var mediator = SetupMediator();

            var childId = Guid.NewGuid();

            await mediator.PublishAsync(new ChildEvent { Id = childId });
            
            RubishBox.Rublish.Count(x => (Guid)x == childId).ShouldBe(2);
        }

        [Fact]
        public async Task TestCombinedEventForParent()
        {
            var mediator = SetupMediator();

            var parentId = Guid.NewGuid();

            await mediator.PublishAsync(new ParentEvent { Id = parentId });
            
            RubishBox.Rublish.Single(x => (Guid)x == parentId);
        }

        private IMediator SetupMediator()
        {
            var builder = new MediatorBuilder();

            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(ParentEvent), typeof(ParentAndChildEventCombinedHandler)),
                    new MessageBinding(typeof(ChildCommand), typeof(ParentAndChildEventCombinedHandler)),
                };
                
                return binding;
            }).Build();
        }
    }
}