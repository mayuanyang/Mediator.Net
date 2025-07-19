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

            await mediator.SendAsync(new ParentCommand(parentId));
            
            RubishBox.Rublish.Single(x => (Guid)x == parentId);
        }

        [Fact]
        public async Task TestCombinedWithResponse()
        {
            var mediator = SetupMediator(true);

            var parentId = Guid.NewGuid();
            var childId = Guid.NewGuid();

            var parentResponse = await mediator.SendAsync<ParentCommand, InheritanceCombinedResponse>(new ParentCommand(parentId));
            var childResponse = await mediator.SendAsync<ChildCommand, InheritanceCombinedResponse>(new ChildCommand(childId));
            
            parentResponse.Id.ShouldBe(parentId);
            childResponse.Id.ShouldBe(childId);
        }

        private IMediator SetupMediator(bool withResponse)
        {
            var builder = new MediatorBuilder();

            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(ParentCommand), withResponse ? typeof(ParentAndChildCommandCombinedWithResponseHandler) : typeof(ParentAndChildCommandCombinedHandler)),
                    new MessageBinding(typeof(ChildCommand), withResponse ? typeof(ParentAndChildCommandCombinedWithResponseHandler) : typeof(ParentAndChildCommandCombinedHandler))
                };

                return binding;
            }).Build();
        }
    }
}