using Autofac;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests
{
    [Collection("Avoid parallel execution")]
    public class TestContainer : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
 
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
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        public void WhenTryToResolveTheInterfaceType()
        {
            _mediator = _container.Resolve<IMediator>();
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
