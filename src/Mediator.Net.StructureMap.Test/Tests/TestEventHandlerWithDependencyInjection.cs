using System;
using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.StructureMap.Test.Tests
{
    public class TestEventHandlerWithDependencyInjection : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private Task _task;
 
        void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterUnduplicatedHandlers()
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
            _container.Configure(mediaBuilder);
        }

        Task WhenACommandIsSent()
        {
            _mediator = _container.GetInstance<IMediator>();
            _task = _mediator.PublishAsync(new SimpleEvent(Guid.NewGuid()));
            return _task;
        }

        void ThenTheEventShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
