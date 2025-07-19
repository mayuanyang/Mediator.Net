using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.TestRequestHandlers;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    public class OneHandlerToHandleMultipleMessagesShouldWork : TestBase
    {
        public OneHandlerToHandleMultipleMessagesShouldWork()
        {
            ClearBinding();
        }

        [Fact]
        public async Task SendCommandExplicitBindingsShouldWork()
        {
            var mediator = SetupCommandMediatorWithExplicitBindings();
            
            await RunCommandTest(mediator);
        }

        [Fact]
        public async Task SendCommandAutoBindingShouldWork()
        {
            var mediator = SetupCommandMediatorWithAutoBindings();
            
            await RunCommandTest(mediator);
        }

        [Fact]
        public async Task PublishEventExplicitBindingsShouldWork()
        {
            var mediator = SetupEventMediatorWithExplicitBindings();
            
            await RunEventTest(mediator);
        }

        [Fact]
        public async Task PublishEventAutoBindingShouldWork()
        {
            var mediator = SetupEventMediatorWithAutoBindings();
            
            await RunEventTest(mediator);
        }

        [Fact]
        public async Task RequestWithExplicitBindingsShouldWork()
        {
            var mediator = SetupRequestMediatorWithAutoBindings();
            
            await RunRequestTest(mediator);
        }

        [Fact]
        public async Task RequestWithAutoBindingShouldWork()
        {
            var mediator = SetupRequestMediatorWithAutoBindings();
            
            await RunRequestTest(mediator);
        }

        async Task RunCommandTest(IMediator mediator)
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            await mediator.SendAsync(new AnotherCommand(id1));
            await mediator.SendAsync(new DerivedTestBaseCommand(id2));
            
            RubishBox.Rublish.Contains(id1).ShouldBe(true);
            RubishBox.Rublish.Contains(id2).ShouldBe(true);
        }

        async Task RunEventTest(IMediator mediator)
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            await mediator.PublishAsync(new SimpleEvent(id1));
            await mediator.PublishAsync(new TestEvent(id2));
            
            RubishBox.Rublish.Contains(id1).ShouldBe(true);
            RubishBox.Rublish.Contains(id2).ShouldBe(true);
        }

        async Task RunRequestTest(IMediator mediator)
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            await mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest(id1.ToString()));
            await mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(id2));
            
            RubishBox.Rublish.Contains(id1.ToString()).ShouldBe(true);
            RubishBox.Rublish.Contains(id2).ShouldBe(true);
        }

        IMediator SetupCommandMediatorWithExplicitBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(AnotherCommand), typeof(MultiCommandsHandler)),
                    new MessageBinding(typeof(DerivedTestBaseCommand), typeof(MultiCommandsHandler))
                };
                
                return binding;
            }).Build();
        }

        IMediator SetupCommandMediatorWithAutoBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(assembly => assembly.DefinedTypes.Where(t => t.Name == nameof(MultiCommandsHandler)), TestUtilAssembly.Assembly).Build();
        }

        IMediator SetupEventMediatorWithExplicitBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(SimpleEvent), typeof(MultiEventsHandler)),
                    new MessageBinding(typeof(TestEvent), typeof(MultiEventsHandler))
                };
                
                return binding;
            }).Build();
        }

        IMediator SetupEventMediatorWithAutoBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(assembly => assembly.DefinedTypes.Where(t => t.Name == nameof(MultiEventsHandler)), TestUtilAssembly.Assembly).Build();
        }

        IMediator SetupRequestMediatorWithExplicitBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>()
                {
                    new MessageBinding(typeof(GetGuidRequest), typeof(MultiRequestsHandler)),
                    new MessageBinding(typeof(SimpleRequest), typeof(MultiRequestsHandler))
                };
                
                return binding;
            }).Build();
        }

        IMediator SetupRequestMediatorWithAutoBindings()
        {
            var builder = new MediatorBuilder();
            
            return builder.RegisterHandlers(assembly => assembly.DefinedTypes.Where(t => t.Name == nameof(MultiRequestsHandler)), TestUtilAssembly.Assembly).Build();
        }
    }
}