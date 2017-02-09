using System;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using NUnit.Framework;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;

namespace Mediator.Net.SimpleInjector.Test.Tests
{

    class TestCommandHandlerWithDependancyInjection : TestBase
    {
        private Container _container = null;
        private IMediator _mediator;
        private Task _task;

        public void GivenAContainer()
        {
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            _container.Register<SimpleService>();
            _container.Register<AnotherSimpleService>();

            InjectHelper.RegisterMediator(_container, mediaBuilder);
        }

        public void WhenACommandIsSent()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
                _task = _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
            }

        }

        public void ThenTheCommandShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);

        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
