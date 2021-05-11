using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
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
        
        [Theory]
        [InlineData("CommandPipe")]
        [InlineData("GlobalPipe")]
        public async Task TestCommandWithUnifiedResult(string whichPipe)
        {
            var builder = SetupMediatorBuilderWithMiddleware(whichPipe);
            var mediator = SetupCommandMediatorWithUnifiedResultMiddleware(builder);
            var response = await 
                mediator.SendAsync<TestCommandWithResponse, UnifiedResponse>(
                    new TestCommandWithResponse());
            
            response.Result.ShouldBeNull();
            response.Error.Code.ShouldBe(12345);
            response.Error.Message.ShouldBe("An error has occured");
        }
        
        [Theory]
        [InlineData("RequestPipe")]
        [InlineData("GlobalPipe")]
        public async Task TestRequestWithUnifiedResult(string whichPipe)
        {
            var builder = SetupMediatorBuilderWithMiddleware(whichPipe);
            var mediator = SetupCommandMediatorWithUnifiedResultMiddleware(builder);
            var response = await 
                mediator.SendAsync<TestCommandWithResponse, UnifiedResponse>(
                    new TestCommandWithResponse());
            
            response.Result.ShouldBeNull();
            response.Error.Code.ShouldBe(12345);
            response.Error.Message.ShouldBe("An error has occured");
        }

        MediatorBuilder SetupMediatorBuilderWithMiddleware(string whichPipe)
        {
            var builder = new MediatorBuilder();
            switch (whichPipe)
            {
                case "CommandPipe":
                    builder.ConfigureCommandReceivePipe(config => config.UseUnifyResultMiddleware(typeof(UnifiedResponse)));
                    break;
                case "RequestPipe":
                    builder.ConfigureRequestPipe(config => config.UseUnifyResultMiddleware(typeof(UnifiedResponse)));
                    break;
                case "GlobalPipe":
                    builder.ConfigureGlobalReceivePipe(config => config.UseUnifyResultMiddleware(typeof(UnifiedResponse)));
                    break;
            }

            return builder;
        }
        
        IMediator SetupCommandMediatorWithUnifiedResultMiddleware(MediatorBuilder builder)
        {
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestCommandWithResponse), typeof(TestCommandWithResponseThatThrowBusinessExceptionHandler)),
                };
                return binding;
            }).Build();
        }
        
        IMediator SetupCommandMediatorWithExplicitBindings()
        {
            var builder = new MediatorBuilder();
            builder.ConfigureCommandReceivePipe(config => config.UseConsoleLogger1());
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