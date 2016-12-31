using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.Autofac.Test.Middlewares;
using NUnit.Framework;
using Mediator.Net;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Autofac.Test
{
   
    class TestContainer
    {
        private IContainer _container = null;
        private IMediator _mediator;
        [TestFixtureSetUp]
        public void Setup()
        {
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(typeof(TestContainer).Assembly)
                .ConfigureReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        public void GivenAContainer()
        {
            
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
