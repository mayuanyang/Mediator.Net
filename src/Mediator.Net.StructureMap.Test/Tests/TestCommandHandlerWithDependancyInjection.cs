using System;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using NUnit.Framework;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;

namespace Mediator.Net.StructureMap.Test.Tests
{
   
    class TestCommandHandlerWithDependancyInjection : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private Task _task;
 
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

        public void WhenACommandIsSent()
        {
            _mediator = _container.GetInstance<IMediator>();
            _task = _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
        }

        public void ThenTheCommandShouldReachItsHandler()
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
