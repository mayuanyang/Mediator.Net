using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class TestInheritanceCombinedHandler
    {
        [Fact]
        public async Task TestCombinedCommandForChild()
        {
            var mediator = SetupMediator(false);

            var childId = Guid.NewGuid();

            await mediator.SendAsync(new ChildCommand(childId));
            RubishBox.Rublish.Single(x => (Guid)x == childId);
        }

        [Fact]
        public async Task TestCombinedCommandForParent()
        {
            var mediator = SetupMediator(false);

            var parentId = Guid.NewGuid();

            await mediator.SendAsync(new InheritanceCommand(parentId));
            RubishBox.Rublish.Single(x => (Guid)x == parentId);
        }

        [Fact]
        public async Task TestCombinedWithResponse()
        {
            var mediator = SetupMediator(true);

            var parentId = Guid.NewGuid();
            var childId = Guid.NewGuid();

            var parentResponse = await mediator.SendAsync<InheritanceCommand, InheritanceCombinedResponse>(new InheritanceCommand(parentId));
            var childResponse = await mediator.SendAsync<ChildCommand, InheritanceCombinedResponse>(new ChildCommand(childId));
            
            parentResponse.Id.ShouldBe(parentId);
            childResponse.Id.ShouldBe(childId);
        }

        private IMediator SetupMediator(bool withResponse)
        {
            var builder = new MediatorBuilder();

            builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(InheritanceCommand), withResponse ? typeof(InheritanceCombinedWithResponseHandler) : typeof(InheritanceCombinedHandler)),
                    new MessageBinding(typeof(ChildCommand), withResponse ? typeof(InheritanceCombinedWithResponseHandler) : typeof(InheritanceCombinedHandler)),
                };


                return binding;
            }).Build();
            return builder.Build();
        }
    }
}