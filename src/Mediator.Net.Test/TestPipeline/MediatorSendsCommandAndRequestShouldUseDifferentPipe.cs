using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class MediatorSendsCommandAndRequestShouldUseDifferentPipe : TestBase
    {
        private IMediator _mediator;
        private GetGuidResponse _result;
        private Task _commandTask;
        private Guid _id = Guid.NewGuid();
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
           var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
                        new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
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
                .ConfigureRequestPipe(x =>
                {
                    x.UseConsoleLogger3();
                })
            .Build();


        }

        async Task WhenACommandAndARequestAreSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
            _result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_id));
        }

        void ThenTheCommandShouldBeHandled()
        {
            
        }

        void AndTheRequestShouldBeHandled()
        {
            _result.Id.ShouldBe(_id);
            RubishBox.Rublish.Count.ShouldBe(6);
            RubishBox.Rublish.Contains(nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog2.UseConsoleLogger2)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(ConsoleLog3.UseConsoleLogger3)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandler)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(GetGuidRequestHandler)).ShouldBeTrue();
        }

        void AndItShouldUseConsoleLogger1Twice()
        {
            RubishBox.Rublish.Count(x => x.ToString() == nameof(ConsoleLog1.UseConsoleLogger1)).ShouldBe(2);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
