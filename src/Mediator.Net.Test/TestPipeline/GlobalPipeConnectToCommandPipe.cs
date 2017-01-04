using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestPipeline
{
    class GlobalPipeConnectToCommandPipe : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        public void GivenAMediator()
        {
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
                .ConfigureReceivePipe(x =>
                {
                    x.UseConsoleLogger2();
                })
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger3();
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
        }

    
        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
