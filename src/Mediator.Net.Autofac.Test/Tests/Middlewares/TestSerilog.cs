using System;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Mediator.Net.Middlewares.Serilog;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Autofac.Test.Tests.Middlewares
{
    class TestSerilog : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;

        public void GivenAContainer()
        {
            var containerBuilder = new ContainerBuilder();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
            .CreateLogger();
            containerBuilder.RegisterInstance(Log.Logger);

            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSerilog(LogEventLevel.Information);
                });
            
            containerBuilder.RegisterMediator(mediaBuilder);
            containerBuilder.RegisterType<SimpleService>();
            containerBuilder.RegisterType<AnotherSimpleService>();
            _container = containerBuilder.Build();
        }

        public async Task WhenACommandIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            await _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
        }

        public void ThenInterfaceTypeShouldBeResolved()
        {
            _mediator.ShouldNotBeNull();

        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
