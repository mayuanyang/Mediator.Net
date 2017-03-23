using System;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net.Autofac.Test.Middlewares;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Services;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Autofac.Test.Tests
{
   
    class TestAllPipelines : TestBase
    {
        private IContainer _container = null;
        private IMediator _mediator;
        
        public void GivenAMediatorBuildConnectsToAllPipelines()
        {
            base.ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureGlobalReceivePipe(global =>
                {
                    global.UseSimpleMiddleware1();
                })
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware1();
                })
                .ConfigureEventReceivePipe(e =>
                {
                    e.UseSimpleMiddleware1();
                })
                .ConfigurePublishPipe(p =>
                {
                    p.UseSimpleMiddleware1();
                }).ConfigureRequestPipe(r =>
                {
                    r.UseSimpleMiddleware1();
                });
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<SimpleService>().AsSelf();
            containerBuilder.RegisterType<AnotherSimpleService>().AsSelf();
            containerBuilder.RegisterMediator(mediaBuilder);
            _container = containerBuilder.Build();
        }

        public async Task WhenAMessageIsSent()
        {
            _mediator = _container.Resolve<IMediator>();
            await _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
            await _mediator.PublishAsync(new SimpleEvent());
            await _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest());

        }

        public void ThenAllMiddlewaresInPipelinesShouldBeExecuted()
        {
            RubishBin.Rublish.Count.ShouldBe(6);
            
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
