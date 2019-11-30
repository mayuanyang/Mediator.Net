using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    public class ShouldTrackStackInPipline : TestBase
    {
        private IMediator _mediator;
        private Exception _exception;

        void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
                {
                    var binding = new List<MessageBinding>()
                    {
                        new MessageBinding(typeof(TrackStackTestCommand), typeof(TrackStackTestCommandHandler))
                    };
                    return binding;
                })
                .Build();
        }

        async Task WhenCommandIsSent()
        {
            try
            {
                await _mediator.SendAsync(new TrackStackTestCommand());
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        void ThenExceptionShouldThroughValidation()
        {
            _exception.ShouldNotBeNull();
            _exception.InnerException.StackTrace.ShouldContain(nameof(TrackStackTestCommandHandler));
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
