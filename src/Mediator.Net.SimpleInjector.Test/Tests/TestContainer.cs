using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.SimpleInjector.Test.Tests
{

    public class TestContainer : TestBase
    {
        private Container _container = null;
        private IMediator _mediator;
 
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
            InjectHelper.RegisterMediator(_container, mediaBuilder);
        }

        void WhenTryToResolveTheInterfaceType()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
            }
        }

        void ThenInterfaceTypeShouldBeResolved()
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
