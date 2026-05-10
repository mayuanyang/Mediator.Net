using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandStreamHandlers
{
    public class StreamCommandHandlerRegisteredViaAssemblyScan : TestBase
    {
        private IMediator _mediator;
        private IAsyncEnumerable<TestCommandResponse> _result;

        void GivenAMediatorRegisteredViaAssemblyScan()
        {
            ClearBinding();

            var builder = new MediatorBuilder();

            // Filter to only the stream handler to avoid multiple-handler conflicts from the shared TestUtil assembly
            _mediator = builder
                .RegisterHandlers(
                    assembly => assembly.DefinedTypes.Where(t => t.AsType() == typeof(TestCommandWithResponseStreamHandler)),
                    Assembly.GetAssembly(typeof(TestCommandWithResponseStreamHandler)))
                .Build();
        }

        Task WhenAStreamCommandIsSent()
        {
            _result = _mediator.CreateStream<TestCommandWithResponse, TestCommandResponse>(new TestCommandWithResponse());
            return Task.CompletedTask;
        }

        async Task ThenAllItemsAreReceived()
        {
            var counter = 0;

            await foreach (var r in _result)
            {
                r.Thing.ShouldBe(counter.ToString());
                counter++;
            }

            counter.ShouldBe(5);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
