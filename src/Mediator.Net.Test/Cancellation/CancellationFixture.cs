using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.RequestHandlers;
using Xunit;

namespace Mediator.Net.Test.Cancellation
{
    public class CancellationFixture : TestBase
    {
        private IMediator _mediator;
        public CancellationFixture()
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

        [Fact]
        public void TokenIsPassToHandler()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()), token);

        }
    }
}
