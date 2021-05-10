using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class TestCommandWithResponseSpec: TestBase
    {
        public TestCommandWithResponseSpec()
        {
            ClearBinding();
        }

        [Fact]
        public async Task CommandCanHaveResponse()
        {
            var mediator = SetupCommandMediatorWithExplicitBindings();
            var response = await 
                mediator.SendAsync<TestCommandWithResponse, TestCommandResponse>(
                    new TestCommandWithResponse());
            
            response.Thing.ShouldBe("Hello world");
        }
        
        IMediator SetupCommandMediatorWithExplicitBindings()
        {
            var builder = new MediatorBuilder();
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestCommandWithResponse), typeof(TestCommandWithResponseHandler)),
                };
                return binding;
            }).Build();
        }
    }
}