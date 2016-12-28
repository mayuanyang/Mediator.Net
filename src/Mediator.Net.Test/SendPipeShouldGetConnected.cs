using System;
using System.Threading.Tasks;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test
{
    class SendPipeShouldGetConnected : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            var receivePipe = builder.RegisterHandlersFor(this.GetType().Assembly)
                .BuildReceivePipe(x =>
            {
                x.UseConsoleLogger1();
                x.UseConsoleLogger2();
            })
            .Build();

            var sendPipe = builder.BuildSendPipe(x =>
                {
                    x.UseConsoleLogger1();
                    x.UseConsoleLogger2();
                })
            .Build();

            _mediator = new Mediator(receivePipe, null, sendPipe);
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
