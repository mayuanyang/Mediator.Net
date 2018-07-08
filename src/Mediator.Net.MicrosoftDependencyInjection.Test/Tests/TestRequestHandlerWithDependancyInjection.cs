using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.MicrosoftDependencyInjection.Test.Tests
{
    public class TestRequestHandlerWithDependancyInjection : TestBase
    {
        private IServiceCollection _container = null;
        private IMediator _mediator;
        private Task _task;
 
        void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
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

        Task ThenTheRequestShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            return Task.CompletedTask;
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
