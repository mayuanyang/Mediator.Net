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
    public class TestRequestHandlerWithDependencyInjection : TestBase
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

        Task WhenARequestIsSent()
        {
            _mediator = _container.BuildServiceProvider().GetService<IMediator>();
            _task = _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest("Hello"));
            return _task;
        }

        void ThenTheRequestShouldReachItsHandler()
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
