using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestContext
{
    public class TestUseCustomReceiveContext : TestBase
    {

        private IMediator _mediator;

        public TestUseCustomReceiveContext()
        {
            ClearBinding();
            
            var builder = new MediatorBuilder();
            
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(AsyncTestBaseCommandHandler)),
                        new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
                    };
                    return binding;
                })
                .Build();
        }


        [Fact]
        public async Task CanSendCommand()
        {
            var context = new CustomReceiveContext<TestBaseCommand>(new TestBaseCommand(Guid.NewGuid()));
            
            await _mediator.SendAsync<TestBaseCommand>(context);
            
            RubishBox.Rublish.Contains(nameof(AsyncTestBaseCommandHandler)).ShouldBe(true);
        }

        [Fact]
        public async Task CanPublishEvent()
        {
            var context = new CustomReceiveContext<TestEvent>(new TestEvent(Guid.NewGuid()));
            
            await _mediator.PublishAsync<TestEvent>(context);
            
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

        [Fact]
        public async Task CanRequest()
        {
            var guid = Guid.NewGuid();
            var context = new CustomReceiveContext<GetGuidRequest>(new GetGuidRequest(guid));
            var result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(context);
            
            result.Id.ShouldBe(guid);
        }
    }

}
