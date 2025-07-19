using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestEventHandlers
{
    public class CreateStreamForEventShouldThrow : TestBase
    {
        private IMediator _mediator;
        private Func<IAsyncEnumerable<TestCommandResponse>> _invokation;

        void GivenAMediator()
        {
            ClearBinding();
            
            var builder = new MediatorBuilder();
            
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)) };
                
                return binding;
            }).Build();
        }

        Task WhenAEventIsPublished()
        {
            _invokation = () => _mediator.CreateStream<TestEvent, TestCommandResponse>(new TestEvent(Guid.NewGuid()));
            
            return Task.CompletedTask;
        }

        void ThenItShouldThrow()
        {
            _invokation.ShouldThrow<NotSupportedException>("IEvent is not supported for CreateStream");
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}