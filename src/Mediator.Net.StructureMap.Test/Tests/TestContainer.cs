using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Middlewares;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.StructureMap.Test.Tests
{
    public class TestContainer : TestBase
    {
        private IContainer _container = null;
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

            _container.Configure(mediaBuilder);
        }

        Task WhenTryToResolveTheInterfaceType()
        {
            _mediator = _container.GetInstance<IMediator>();
            return Task.FromResult(0);
        }

        Task ThenInterfaceTypeShouldBeResolved()
        {
            _mediator.ShouldNotBeNull();
            return Task.FromResult(0);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
