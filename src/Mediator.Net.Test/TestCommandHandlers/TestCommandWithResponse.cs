using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
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

        [Fact]
        public async Task TestCommandWithResponseRegistration()
        {
            var mediator = SetupCommandMediatorWithScan();
            var response = await 
                mediator.SendAsync<TestCommandWithResponse, TestCommandResponse>(
                    new TestCommandWithResponse());
            
            response.Thing.ShouldBe("Hello world");
        }
        
        [Theory]
        [InlineData("CommandPipe", true, "", false, false, 12345, "An error has occured")]
        [InlineData("GlobalPipe", true, "", false, false, 12345, "An error has occured")]
        [InlineData("GenericCommandPipe", true, "", false, false, 12345, "An error has occured")]
        [InlineData("GenericGlobalPipe", true, "", false, false, 12345, "An error has occured")]
        [InlineData("CommandPipe", false, "1", false, false, 12345, "")]
        [InlineData("GlobalPipe", false, "2", false, false, 12345, "")]
        [InlineData("GenericCommandPipe", false, "3", false, false, 12345, "")]
        [InlineData("GenericGlobalPipe", false, "4", false, false, 12345, "")]
        [InlineData("GenericGlobalPipe", false, "4", true, false, 12345, "An error has occured")]
        [InlineData("GenericGlobalPipe", false, "4", true, true, 50002, "Error from event handler")]
        public async Task TestCommandWithUnifiedResult(
            string whichPipe, 
            bool shouldThrow, 
            string request, 
            bool shouldPublishEvent, 
            bool shouldEventHandlerThrow,
            int errorCode,
            string errorMessage)
        {
            var builder = SetupMediatorBuilderWithMiddleware(whichPipe);
            var mediator = SetupCommandMediatorWithUnifiedResultMiddleware(builder, whichPipe.Contains("Generic"));

            if (whichPipe.Contains("Generic"))
            {
                var response = await 
                    mediator.SendAsync<TestCommandWithResponse, GenericUnifiedResponse<string>>(
                        new TestCommandWithResponse()
                        {
                            ShouldThrow = shouldThrow, 
                            Request = request,
                            ShouldPublishEvent = shouldPublishEvent,
                            ShouldEventHandlerThrow = shouldEventHandlerThrow
                        });
                if (shouldThrow || shouldEventHandlerThrow)
                {
                    response.Result.ShouldBeNull();
                    AssertErrorResult(response.Error, errorCode, errorMessage);    
                }
                else
                {
                    response.Error.ShouldBeNull();
                    response.Result.ShouldBe(request + "Result");
                }
            }
            else
            {
                var response = await 
                    mediator.SendAsync<TestCommandWithResponse, UnifiedResponse>(
                        new TestCommandWithResponse()
                        {
                            ShouldThrow = shouldThrow, 
                            Request = request,
                            ShouldPublishEvent = shouldPublishEvent,
                            ShouldEventHandlerThrow = shouldEventHandlerThrow

                        });
                if (shouldThrow)
                {
                    response.Result.ShouldBeNull();
                    AssertErrorResult(response.Error, errorCode, errorMessage);    
                }
                else
                {
                    response.Error.ShouldBeNull();
                    response.Result.ShouldBe(request + "Result");
                }
                
            }

            void AssertErrorResult(Error error, int expectedErrorCode, string expectedErrorMessage)
            {
                error.Code.ShouldBe(expectedErrorCode);
                error.Message.ShouldBe(expectedErrorMessage);
            }
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
                case "GenericCommandPipe":
                    builder.ConfigureCommandReceivePipe(config => config.UseUnifyResultMiddleware(typeof(GenericUnifiedResponse<>)));
                    break;
                case "GenericGlobalPipe":
                    builder.ConfigureGlobalReceivePipe(config => config.UseUnifyResultMiddleware(typeof(GenericUnifiedResponse<>)));
                    break;
            }

            return builder;
        }
        
        IMediator SetupCommandMediatorWithUnifiedResultMiddleware(MediatorBuilder builder, bool isGeneric)
        {
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>();
                binding.Add(isGeneric
                    ? new MessageBinding(typeof(TestCommandWithResponse),
                        typeof(TestCommandWithGenericHandler))
                    : new MessageBinding(typeof(TestCommandWithResponse), typeof(TestCommandWithResponseThatThrowBusinessExceptionHandler)));

                binding.Add(new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)));
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

        IMediator SetupCommandMediatorWithScan()
        {
            var builder = new MediatorBuilder();
            return builder
                .RegisterHandlers(assembly => assembly.DefinedTypes.Where(t => t.Name == nameof(TestCommandWithResponseHandler)),
                    typeof(TestCommandWithResponse).GetTypeInfo().Assembly).Build();
        }
    }
}