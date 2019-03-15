using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestRequestHandlers
{
    public class TestAsyncRequestHandler : TestBase
    {
        private IMediator _mediator;

        [Fact]
        public async Task CanRequestInAsync()
        {
            CreateMediator();
            var id = Guid.NewGuid();
            var response =
                await _mediator.RequestAsync<GetGuidInAsyncRequest, GetGuidResponse>(new GetGuidInAsyncRequest(id));
            response.Id.ShouldBe(id);
        }

        private void CreateMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(GetGuidInAsyncRequest), typeof(AsyncRequestHandler))
                    };
                    return binding;
                })
                .Build();
        }
    }
}
