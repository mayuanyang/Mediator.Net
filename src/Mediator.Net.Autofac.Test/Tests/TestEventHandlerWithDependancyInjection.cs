using System;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.Autofac.Test.Messages;
using Mediator.Net.Autofac.Test.Middlewares;
using Mediator.Net.Autofac.Test.Services;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Autofac.Test.Tests
{
   
    class TestEventHandlerWithDependancyInjection : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private Task _task;
 
        public void GivenAContainer()
        {
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(typeof(TestContainer).Assembly)
                .ConfigureReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SimpleService>().AsSelf();
            containerBuilder.RegisterType<AnotherSimpleService>().AsSelf();
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        public void WhenACommandIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _task = _mediator.PublishAsync(new SimpleEvent());
        }

        public void ThenTheEventShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
