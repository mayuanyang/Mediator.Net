using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestPipeline
{
    class MiddlewaresShouldBeExecutedInOrder : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediatorAndTwoMiddlewares()
        {
            var binding = new List<MessageBinding>() { new MessageBinding( typeof(TestBaseCommand), typeof(TestBaseCommandHandler) )};
            var builder = new MediatorBuilder();
            var receivePipe = builder.RegisterHandlers(binding)
                .BuildPipe(x =>
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

        public void ThenTheMiddlewaresShouldBeExecutedInOrder()
        {
            
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
