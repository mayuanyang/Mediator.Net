using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.Autofac.Test.Middlewares;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Services;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests
{
    public class TestRequestPipeWithExceptionEnricherMiddleware : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        private SimpleResponse _simpleResponse;
        
        void GivenAMediatorConnectsToPipelines()
        {
            base.ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterUnduplicatedHandlers()
                .ConfigureGlobalReceivePipe(global =>
                {
                    global.UseSimpleMiddleware1();
                })
                .ConfigureRequestPipe(r =>
                {
                    r.UseEnrichResultOnException();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterMediator(mediaBuilder);
            containerBuilder.RegisterType<SimpleService>().AsSelf();
            containerBuilder.RegisterType<AnotherSimpleService>().AsSelf();
            _container = containerBuilder.Build();
        }

        async Task WhenAMessageIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            _simpleResponse = await _mediator.RequestAsync<SimpleRequestWillThrow, SimpleResponse>(new SimpleRequestWillThrow("Hello"));
        }

        void ThenAllMiddlewaresInPipelinesShouldBeExecuted()
        {
            _simpleResponse.EchoMessage.ShouldBe("Error has occured");
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
