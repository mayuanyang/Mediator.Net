using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    public class GlobalPipeConnectToCommandAndPublishPipe : TestBase
    {
        private IMediator _mediator;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        
        void GivenAMediator()
        {
            ClearBinding();
            
           var builder = new MediatorBuilder();
           
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
                        new MessageBinding(typeof(TestEvent), typeof(TestEventHandler))
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
                .ConfigurePublishPipe(x =>
                {
                    x.UseConsoleLogger3();
                })
            .Build();
        }

        Task WhenACommandIsSent()
        {
            _commandTask = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            
            return _commandTask;
        }

        void ThenTheCommandShouldBeHandled()
        {
            _commandTask.Status.ShouldBe(TaskStatus.RanToCompletion);
            
            RubishBox.Rublish.Count.ShouldBe(6);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBe(2);
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog3.UseConsoleLogger3)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandlerRaiseEvent)).ShouldBeTrue();
        }
        
        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}