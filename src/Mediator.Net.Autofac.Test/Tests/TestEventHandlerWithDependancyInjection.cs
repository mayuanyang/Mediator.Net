using System.Threading.Tasks;
using Autofac;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests
{
   
    public class TestEventHandlerWithDependancyInjection : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private Task _task;
 
        public void GivenAContainer()
        {
            base.ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SimpleService>().AsSelf();
            containerBuilder.RegisterType<AnotherSimpleService>().AsSelf();
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        public void WhenACommandIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _task = _mediator.PublishAsync(new SimpleEvent());
        }

        public void ThenTheEventShouldReachItsHandler()
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
