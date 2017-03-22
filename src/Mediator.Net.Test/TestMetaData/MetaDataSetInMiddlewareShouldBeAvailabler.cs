using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestMetaData
{
    class MetaDataSetInMiddlewareShouldBeAvailable : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(Simple2CommandHandler)) };
                return binding;
            })
            .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseSecurityInfo();
                })
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSecurityInfo2();
                })
            .Build();

        }

        public async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish[0].ShouldBe("hello");
            RubishBox.Rublish[1].ShouldBe("password");
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
