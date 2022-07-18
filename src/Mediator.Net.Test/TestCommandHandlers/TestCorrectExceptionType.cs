using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestCommandHandlers
{

    public class TestCorrectExceptionType : TestBase
    {
        [Fact]
        public async Task ShouldHaveTheRightException()
        {
            var builder = new MediatorBuilder();
            builder.ConfigureGlobalReceivePipe(config => config.UseConsoleLogger1())
                .ConfigureCommandReceivePipe(config => config.UseConsoleLogger2());
            var mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(TestCommandWithResponse), typeof(TestCommandWithResponseThatThrowHandler)),
                };
                return binding;
            }).Build();

            try
            {
                await mediator.SendAsync<TestCommandWithResponse, TestCommandResponse>(new TestCommandWithResponse());
            }
            catch (ArgumentException e)
            {
                e.Message.ShouldBe("abc");
            }
            
        }
    }
}