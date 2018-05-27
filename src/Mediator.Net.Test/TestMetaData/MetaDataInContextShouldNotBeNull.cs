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

namespace Mediator.Net.Test.TestMetaData
{
    
    public class MetadataInContextShouldNotBeNull : TestBase
    {
        private IMediator _mediator;
        void GivenAMediator()
        {
            ClearBinding();
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(TestBaseCommand), typeof(SimpleCommandHandler)) };
                return binding;
            })
            .Build();

        }

        void WhenACommandIsSent()
        {
            _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid())).Wait();
        }

        void ThenItShouldReachTheRightHandler()
        {
            RubishBox.Rublish.Count.ShouldBe(1);
            RubishBox.Rublish[0].ShouldBe(false);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
