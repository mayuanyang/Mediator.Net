﻿using System;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Services;
using Mediator.Net.Middlewares.Serilog;
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

            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureGlobalReceivePipe(x => x.UseSerilog())
                .ConfigureCommandReceivePipe(y => y.UseSerilog())
                .ConfigureEventReceivePipe(z => z.UseSerilog())
                .ConfigureRequestPipe(x => x.UseSerilog());
            
            containerBuilder.RegisterMediator(mediaBuilder);
            containerBuilder.RegisterType<SimpleService>();
            containerBuilder.RegisterType<AnotherSimpleService>();
            _container = containerBuilder.Build();
        }

        void WhenACommandAndEventAreSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _mediator.SendAsync(new SimpleCommand(Guid.NewGuid())).Wait();
            _mediator.PublishAsync(new SimpleEvent()).Wait();
            _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest()).Wait();
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
