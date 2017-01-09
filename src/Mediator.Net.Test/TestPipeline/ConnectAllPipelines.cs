using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;


namespace Mediator.Net.Test.TestPipeline
{
    class ConnectAllPipelines : TestBase
    {
        private IMediator _mediator;
        private Guid _id = Guid.NewGuid();
        public void GivenAMediator()
        {
           var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
                        new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
                    };
                    return binding;
                })
                .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseDummySave();
                })
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseConsoleLogger1();
                })
                .ConfigureEventReceivePipe(x =>
                {
                    x.UseConsoleLogger2();
                })
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger3();
                })
                .ConfigurePublishPipe(x =>
                {
                    x.UseConsoleLogger4();
                })
            .Build();


        }

        public async Task WhenAllMessagesAreSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            await _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
            await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(Guid.NewGuid()));
            
        }

        public void ThenTheRightMiddlewaresShouldBeUsed()
        {
            
            RubishBox.Rublish.Count.ShouldBe(13);
            RubishBox.Rublish.Contains(nameof(DummySave.UseDummySave)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog3.UseConsoleLogger3)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog4.UseConsoleLogger4)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandlerRaiseEvent)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(GetGuidRequestHandler)).ShouldBeTrue();

            RubishBox.Rublish.Count(x => x.ToString() == nameof(DummySave.UseDummySave)).ShouldBe(4);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBe(1);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBe(2);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog3.UseConsoleLogger3)).ShouldBe(1);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog4.UseConsoleLogger4)).ShouldBe(1);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(TestBaseCommandHandlerRaiseEvent)).ShouldBe(1);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(TestEventHandler)).ShouldBe(2);
            RubishBox.Rublish.Count(x => x.ToString() == nameof(GetGuidRequestHandler)).ShouldBe(1);
          
        }

        public void AndTheMiddlewaresShouldBeUsedInTheCorrectOrder()
        {
            RubishBox.Rublish[0].ShouldBe(nameof(DummySave.UseDummySave));
            RubishBox.Rublish[1].ShouldBe(nameof(ConsoleLog1.UseConsoleLogger1));
            RubishBox.Rublish[2].ShouldBe(nameof(TestBaseCommandHandlerRaiseEvent));
            RubishBox.Rublish[3].ShouldBe(nameof(ConsoleLog4.UseConsoleLogger4));

            RubishBox.Rublish[4].ShouldBe(nameof(DummySave.UseDummySave));
            RubishBox.Rublish[5].ShouldBe(nameof(ConsoleLog2.UseConsoleLogger2));
            RubishBox.Rublish[6].ShouldBe(nameof(TestEventHandler));

            RubishBox.Rublish[7].ShouldBe(nameof(DummySave.UseDummySave));
            RubishBox.Rublish[8].ShouldBe(nameof(ConsoleLog2.UseConsoleLogger2));
            RubishBox.Rublish[9].ShouldBe(nameof(TestEventHandler));

            RubishBox.Rublish[10].ShouldBe(nameof(DummySave.UseDummySave));
            RubishBox.Rublish[11].ShouldBe(nameof(ConsoleLog3.UseConsoleLogger3));
            RubishBox.Rublish[12].ShouldBe(nameof(GetGuidRequestHandler));
        }

    
        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
