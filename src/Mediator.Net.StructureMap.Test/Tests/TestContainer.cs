using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Middlewares;
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
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterUnduplicatedHandlers()
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
