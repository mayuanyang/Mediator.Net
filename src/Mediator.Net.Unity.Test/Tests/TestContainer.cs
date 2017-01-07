using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Unity.Test.Tests
{
   
    class TestContainer : TestBase
    {
        private IUnityContainer _container = null;
        private IMediator _mediator;
 
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

        public void WhenTryToResolveTheInterfaceType()
        {
            _mediator = _container.Resolve<IMediator>();
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
