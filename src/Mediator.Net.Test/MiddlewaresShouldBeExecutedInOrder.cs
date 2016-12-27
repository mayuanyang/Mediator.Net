using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test
{
    class MiddlewaresShouldBeExecutedInOrder : TestBase
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
            
            _mediator = new Mediator(receivePipe, null);
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
