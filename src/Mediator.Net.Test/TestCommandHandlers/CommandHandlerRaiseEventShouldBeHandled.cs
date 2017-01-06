using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.EventHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestCommandHandlers
{
    class CommandHandlerRaiseEventShouldBeHandled : TestBase
    {
        private IMediator _mediator;
        private Task _task;
        public void GivenAMediator()
        {

            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
                    new MessageBinding(typeof(TestEvent), typeof(TestEventHandler))
                };
                return binding;
            })
            .ConfigurePublishPipe(x =>
                {
                    x.UseDummySave();
                })
            .Build();

        }

        public void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));

        }

        public async Task ThenItShouldReachTheRightHandler()
        {
            await _task;
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
        }

        public void AndItShouldPutSomeRubishIntoRublishBox()
        {
            RubishBox.Rublish.Contains(nameof(DummySave.UseDummySave)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandlerRaiseEvent)).ShouldBeTrue();
            RubishBox.Rublish.Contains(nameof(TestEventHandler)).ShouldBeTrue();
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
