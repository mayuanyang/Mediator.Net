using System.Threading.Tasks;
using Autofac;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.Services;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests
{
    public class TestRequestHandlerWithDependancyInjection : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private Task _task;
 
        void GivenAContainer()
        {
            base.ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureRequestPipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SimpleService>().AsSelf();
            containerBuilder.RegisterType<AnotherSimpleService>().AsSelf();
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        Task WhenARequestIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _task = _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest("Hello"));
            return _task;
        }

        void ThenTheRequestShouldReachItsHandler()
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
