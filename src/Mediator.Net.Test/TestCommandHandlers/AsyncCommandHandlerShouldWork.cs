using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class AsyncCommandHandlerShouldWork : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(AsyncTestBaseCommandHandler)) };
                return binding;
            })
            .Build();

        }

        public async Task WhenACommandIsSent()
        {
            var sw = new Stopwatch();
            sw.Start();
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            sw.Stop();
            Console.WriteLine($"it took {sw.ElapsedMilliseconds} milliseconds");
        }

        public void ThenItShouldReachTheRightHandler()
        {

        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
