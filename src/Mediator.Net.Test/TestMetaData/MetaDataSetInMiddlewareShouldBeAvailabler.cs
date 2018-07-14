using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestMetaData
{
    
    public class MetaDataSetInMiddlewareShouldBeAvailable : TestBase
    {
        private IMediator _mediator;
        void GivenAMediator()
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

        async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
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
