using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestInheritance
{
    public class TestInheritanceSpec : TestBase
    {
        private  Guid _id = Guid.NewGuid();
        private IMediator _mediator;
       

        [Fact]
        public async Task TestCommandHandler()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(InheritanceCommand), typeof(ChildCommandHandler)) };
                return binding;
            }).Build();

            await _mediator.SendAsync(new InheritanceCommand(_id));

            RubishBox.Rublish.Count.ShouldBe(2);
            RubishBox.Rublish[0].ShouldBe(_id);
            RubishBox.Rublish[1].ShouldBe("From parent");
        }

        [Fact]
        public async Task TestRequestHandler()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(InheritanceRequest), typeof(ChildRequestHandler)) };
                return binding;
            }).Build();

            var response = await _mediator.RequestAsync<InheritanceRequest, InheritanceResponse>(new InheritanceRequest(_id));

            response.Id.ShouldBe(_id);
            response.Content.ShouldBe("Hello world");
        }
    }
}
