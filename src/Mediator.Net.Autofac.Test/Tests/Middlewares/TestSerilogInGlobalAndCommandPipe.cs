using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.Middlewares.Serilog;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;
using Serilog;
using TestStack.BDDfy;
using NSubstitute;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests.Middlewares
{
    public class TestSerilogInGlobalAndCommandPipe : TestBase
    {
        private IContainer _container;
        private IMediator _mediator;
        private ILogger _logger;

        void GivenAMediatorWithSerilogAddToAllPipelines()
        {
            ClearBinding();
            var containerBuilder = new ContainerBuilder();
            _logger = Substitute.For<ILogger>();
            containerBuilder.RegisterInstance(_logger).As<ILogger>();

            var mediaBuilder = new MediatorBuilder();

            mediaBuilder.RegisterUnduplicatedHandlers()
                .ConfigureGlobalReceivePipe(x => x.UseSerilog())
                .ConfigureCommandReceivePipe(y => y.UseSerilog())
                .ConfigureEventReceivePipe(z => z.UseSerilog())
                .ConfigureRequestPipe(x => x.UseSerilog());
            
            containerBuilder.RegisterMediator(mediaBuilder);
            containerBuilder.RegisterType<SimpleService>();
            containerBuilder.RegisterType<AnotherSimpleService>();
            _container = containerBuilder.Build();
        }

        async Task WhenACommandAndEventAreSent()
        {
            _mediator = _container.Resolve<IMediator>();
            await _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
            await _mediator.PublishAsync(new SimpleEvent(Guid.NewGuid()));
            await _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest("Hello"));
        }

        void ThenItShouldLogTheCommand()
        {
            _logger.Received(2).Information(Arg.Any<string>(), Arg.Any<SimpleCommand>());
            _logger.Received(2).Information(Arg.Any<string>(), Arg.Any<SimpleEvent>());
            _logger.Received(2).Information(Arg.Any<string>(), Arg.Any<SimpleRequest>());
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
