using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Unity.Test.Tests
{
   
    class TestCommandHandlerWithDependancyInjection : TestBase
    {
        private IUnityContainer _container = null;
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
            _container = new UnityContainer();
            _container.RegisterType<SimpleService>();
            _container.RegisterType<AnotherSimpleService>();

            UnityExtensioins.Configure(mediaBuilder, _container);
        }

        public void WhenACommandIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
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
