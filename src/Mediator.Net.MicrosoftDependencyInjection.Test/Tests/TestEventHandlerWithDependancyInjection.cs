using System;
using System.Threading.Tasks;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Mediator.Net.TestUtil.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.MicrosoftDependencyInjection.Test.Tests
{
    public class TestEventHandlerWithDependancyInjection : TestBase
    {
        private IServiceCollection _container = null;
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
            _container = new ServiceCollection()
                .AddTransient<SimpleService>()
                .AddTransient<AnotherSimpleService>();
            _container.RegisterMediator(mediaBuilder);
        }

        Task WhenACommandIsSent()
        {
            _mediator = _container.BuildServiceProvider().GetService<IMediator>();
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
