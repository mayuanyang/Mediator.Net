using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Unity.Test.Tests
{
   
    class TestRequestHandlerWithDependancyInjection : TestBase
    {
        private IUnityContainer _container = null;
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
            _container = new UnityContainer();
            _container.RegisterType<SimpleService>();
            _container.RegisterType<AnotherSimpleService>();

            UnityExtensioins.Configure(mediaBuilder, _container);


        }

        public void WhenARequestIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _task = _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest());
        }

        public void ThenTheRequestShouldReachItsHandler()
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
