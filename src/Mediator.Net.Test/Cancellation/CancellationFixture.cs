using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mediator.Net.Binding;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.Cancellation
{
    public class CancellationFixture : TestBase
    {
        private IMediator _mediator;
        public CancellationFixture()
        {
            ClearBinding();
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

        [Fact]
        public void TokenIsPassWhenACommandIsSent()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()), token).Wait();
            TokenRecorder.Recorder.Any(hashcode => hashcode != token.GetHashCode()).ShouldBe(false);

            // Each middleware has 3, go through the global one twice
            TokenRecorder.Recorder.Count.ShouldBe(17);

        }

        [Fact]
        public void TokenIsPassWhenAEventIsSent()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            _mediator.PublishAsync(new TestEvent(Guid.NewGuid()), token).Wait();
            TokenRecorder.Recorder.Any(hashcode => hashcode != token.GetHashCode()).ShouldBe(false);
            TokenRecorder.Recorder.Count.ShouldBe(7);

        }

        [Fact]
        public void TokenIsPassWhenARequestIsSent()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(Guid.NewGuid()), token).Wait();
            TokenRecorder.Recorder.Any(hashcode => hashcode != token.GetHashCode()).ShouldBe(false);
            TokenRecorder.Recorder.Count.ShouldBe(7);

        }
    }
}
