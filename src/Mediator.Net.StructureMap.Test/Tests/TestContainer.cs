using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using NUnit.Framework;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;

namespace Mediator.Net.StructureMap.Test.Tests
{
   
    class TestContainer : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
 
        public void GivenAContainer()
        {
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
            _container.Configure(x =>
            {
                x.ForConcreteType<SimpleService>();
                x.ForConcreteType<AnotherSimpleService>();
            });
            StructureMapExtensions.Configure(mediaBuilder, _container);
        }

        public void WhenTryToResolveTheInterfaceType()
        {
            _mediator = _container.GetInstance<IMediator>();
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
