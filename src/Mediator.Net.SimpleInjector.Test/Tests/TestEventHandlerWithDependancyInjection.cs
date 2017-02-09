using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using NUnit.Framework;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;

namespace Mediator.Net.SimpleInjector.Test.Tests
{
   
    class TestEventHandlerWithDependancyInjection : TestBase
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
            InjectHelper.RegisterMediator(_container, mediaBuilder);
        }

        public void WhenACommandIsSent()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
            _task = _mediator.PublishAsync(new SimpleEvent());
            }
            
        }

        public void ThenTheEventShouldReachItsHandler()
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
