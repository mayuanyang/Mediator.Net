using System;
using System.Threading.Tasks;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test
{
    class SendPipeInterConnectedToReceivePipe : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            var sendPipe = builder.BuildPipe(x =>
            {
                x.UseConsoleLogger1();
                x.UseConsoleLogger2();
            })
            .Build();

            var receivePipe = builder.RegisterHandlersFor(this.GetType().Assembly)
                .BuildPipe(x =>
            {
                x.UseConsoleLogger3();
            })
            .Build();

            

            _mediator = new Mediator(receivePipe, null, sendPipe, ConnectionMode.InterConnect);
        }

        public async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
        {
            
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
