using System;
using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.Services;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.SimpleInjector.Test.Tests
{

    public class TestCommandHandlerWithDependencyInjection : TestBase
    {
        private Container _container = null;
        private IMediator _mediator;
        private Task _task;

        void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterUnduplicatedHandlers()
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            _container.Register<SimpleService>();
            _container.Register<AnotherSimpleService>();

            _container.RegisterMediator(mediaBuilder);
        }

        Task WhenACommandIsSent()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
                _task = _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
                return _task;
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
