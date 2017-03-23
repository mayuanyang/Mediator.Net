using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    public class GlobalPipeConnectToCommandPipe : TestBase
    {
        private IMediator _mediator;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        public void GivenAMediator()
        {
            ClearBinding();
           var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseConsoleLogger1();
                })
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseConsoleLogger2();
                })
            .Build();


        }

        public void WhenACommandIsSent()
        {
            _commandTask = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            
        }

        public void ThenTheCommandShouldBeHandled()
        {
            _commandTask.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Count.ShouldBe(3);
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandler)).ShouldBeTrue();
        }

    
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
