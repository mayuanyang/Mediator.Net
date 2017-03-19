using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.StructureMap.Test.Tests
{
   
    class TestContainer : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
 
        public void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
           
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

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
