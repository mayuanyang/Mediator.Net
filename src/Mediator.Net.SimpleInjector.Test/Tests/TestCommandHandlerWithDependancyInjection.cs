using System;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.SimpleInjector.Test.Tests
{

    public class TestCommandHandlerWithDependancyInjection : TestBase
    {
        private Container _container = null;
        private IMediator _mediator;
        private Task _task;

        void GivenAContainer()
        {
            ClearBinding();
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

        void WhenACommandIsSent()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
                _task = _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
            }

        }

        void ThenTheCommandShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);

        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
