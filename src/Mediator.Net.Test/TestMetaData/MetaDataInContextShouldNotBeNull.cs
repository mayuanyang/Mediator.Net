using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestMetaData
{
    class MetadataInContextShouldNotBeNull : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {

            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(SimpleCommandHandler)) };
                return binding;
            })
            .Build();

        }

        public async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(1);
            RubishBox.Rublish[0].ShouldBe(false);
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
