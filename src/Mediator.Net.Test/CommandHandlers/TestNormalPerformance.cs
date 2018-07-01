using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.Messages;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.CommandHandlers
{
    public class TestNormalPerformance : TestBase
    {
        private TestBaseCommandHandler _handler;
        private Stopwatch _sw;

        void GivenAHandler()
        {
            ClearBinding();
            _sw = new Stopwatch();
            _sw.Start();
            _handler = new TestBaseCommandHandler();
        }

        async Task WhenTheCommandIsHandled()
        {
            var cmd = new TestBaseCommand(Guid.NewGuid());
            var context = new ReceiveContext<TestBaseCommand>(cmd);
            var console1 = new EmptyPipeSpecification<IReceiveContext<TestBaseCommand>>();
            var console2 = new EmptyPipeSpecification<IReceiveContext<TestBaseCommand>>();
            
            
            await console1.ExecuteBeforeConnect(context, default(CancellationToken));
            await console2.ExecuteBeforeConnect(context, default(CancellationToken));
            await _handler.Handle(new ReceiveContext<TestBaseCommand>(cmd), default(CancellationToken));
            await console1.ExecuteAfterConnect(context, default(CancellationToken));
            await console2.ExecuteAfterConnect(context, default(CancellationToken));
            _sw.Stop();
            Console.WriteLine($"It took {_sw.ElapsedMilliseconds} milliseconds");
        }

        void ThenItShouldBeFast()
        {
            
        }
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
