using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.Messages;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test.CommandHandlers
{
    class TestNormalPerformance
    {
        private TestBaseCommandHandler _handler;
        public void GivenAHandler()
        {
            _handler = new TestBaseCommandHandler();
        }

        public async Task WhenTheCommandIsHandled()
        {
           
            var sw = new Stopwatch();
            sw.Start();
            var cmd = new TestBaseCommand(Guid.NewGuid());
            var context = new ReceiveContext<TestBaseCommand>(cmd);
            var console1 = new EmptyPipeSpecification<IReceiveContext<TestBaseCommand>>();
            var console2 = new EmptyPipeSpecification<IReceiveContext<TestBaseCommand>>();
            
            
            await console1.ExecuteBeforeConnect(context);
            await console2.ExecuteBeforeConnect(context);
            await _handler.Handle(new ReceiveContext<TestBaseCommand>(cmd));
            await console1.ExecuteAfterConnect(context);
            await console2.ExecuteAfterConnect(context);
            sw.Stop();
            Console.WriteLine($"It took {sw.ElapsedMilliseconds} milliseconds");
        }

        public void ThenItShouldBeFast()
        {
            
        }
        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
