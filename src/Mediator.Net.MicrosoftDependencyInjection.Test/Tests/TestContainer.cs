using System.ComponentModel;
using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.MicrosoftDependencyInjection.Test.Tests
{
    public class TestContainer : TestBase
    {
        private IServiceCollection _container = null;
        private IMediator _mediator;
 
        void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterUnduplicatedHandlers()
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new ServiceCollection();
           
            _container.RegisterMediator(mediaBuilder);
        }

        Task WhenTryToResolveTheInterfaceType()
        {
            _mediator = _container.BuildServiceProvider().GetService<IMediator>();
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
