using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class MiddlewaresShouldBeExecutedInReverseOrder : TestBase
    {
        private IMediator _mediator;
        void GivenAMediator()
        {
            ClearBinding();
            var binding = new List<MessageBinding>() { new MessageBinding( typeof(TestBaseCommand), typeof(TestBaseCommandHandler) ) };
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(binding)
                .ConfigureCommandReceivePipe(x =>
            {
                x.UseConsoleLogger2();
                x.UseConsoleLogger1();
            })
            .Build();
 
        }

        async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish[0].ShouldBe(nameof(ConsoleLog2.UseConsoleLogger2));
            RubishBox.Rublish[1].ShouldBe(nameof(ConsoleLog1.UseConsoleLogger1));
            
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
