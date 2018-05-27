﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{
    
    public class CommandShouldBeSendToItsHandler : TestBase
    {
        private IMediator _mediator;
        private Task _task;

        public CommandShouldBeSendToItsHandler()
        {
            ClearBinding();
        }
        void GivenAMediatorWithNoPipeline()
        {
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)) };
                return binding;
            }).Build();

        }

        void WhenACommandIsSent()
        {
            _task = _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));

        }

        void ThenItShouldReachTheRightHandler()
        {
            _task.Wait();
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            RubishBox.Rublish.Contains(nameof(TestBaseCommandHandler)).ShouldBeTrue();
            RubishBox.Rublish.Count.ShouldBe(1);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
