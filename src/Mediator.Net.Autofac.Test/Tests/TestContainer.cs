using Autofac;
using Mediator.Net.Autofac.Test.Middlewares;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Autofac.Test.Tests
{
   
    class TestContainer : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
 
        public void GivenAContainer()
        {
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(typeof(TestContainer).Assembly)
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

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
