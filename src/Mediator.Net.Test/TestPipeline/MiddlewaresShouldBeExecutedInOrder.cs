﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.Test.TestUtils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPipeline
{
    
    public class MiddlewaresShouldBeExecutedInOrder : TestBase
    {
        private IMediator _mediator;
        void GivenAMediatorAndTwoMiddlewares()
        {
            ClearBinding();
            var binding = new List<MessageBinding>() { new MessageBinding( typeof(TestBaseCommand), typeof(TestBaseCommandHandler) )};
            var builder = new MediatorBuilder();
            _mediator = builder.RegisterHandlers(binding)
                .ConfigureCommandReceivePipe(x =>
            {
                x.UseConsoleLogger1();
                x.UseConsoleLogger2();
            })
            .Build();
            
           
        }

        void WhenACommandIsSent()
        {
            _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid())).Wait();
        }

        void ThenTheMiddlewaresShouldBeExecutedInOrder()
        {
            RubishBox.Rublish[0].ShouldBe(nameof(ConsoleLog1.UseConsoleLogger1));
            RubishBox.Rublish[1].ShouldBe(nameof(ConsoleLog2.UseConsoleLogger2));
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
