using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using Xunit;

namespace Mediator.Net.Test.TestException
{

    public class TestCorrectExceptionType : TestBase
    {
        [Fact]
        public async Task CommandShouldHaveTheRightException()
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

            bool testChecked = false;
            try
            {
                await mediator.SendAsync<TestCommandWithResponse, TestCommandResponse>(new TestCommandWithResponse());
            }
            catch (ArgumentException e)
            {
                e.Message.ShouldBe("abc");
                testChecked = true;
            }
            testChecked.ShouldBeTrue();
        }
        
        [Fact]
        public async Task RequestShouldHaveTheRightException()
        {
            var builder = new MediatorBuilder();
            builder.ConfigureGlobalReceivePipe(config => config.UseConsoleLogger1())
                .ConfigureRequestPipe(config => config.UseConsoleLogger2());
            var mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(SimpleRequest2), typeof(SimpleRequestThrowArgumentExceptionHandler)),
                };
                return binding;
            }).Build();

            bool testChecked = false;
            try
            {
                await mediator.RequestAsync<SimpleRequest2, SimpleResponse>(new SimpleRequest2());
            }
            catch (ArgumentException e)
            {
                e.Message.ShouldBe("cba");
                testChecked = true;
            }
            testChecked.ShouldBeTrue();
        }
        
        [Fact]
        public async Task EventShouldHaveTheRightException()
        {
            var builder = new MediatorBuilder();
            builder.ConfigureGlobalReceivePipe(config => config.UseConsoleLogger1())
                .ConfigureEventReceivePipe(config => config.UseConsoleLogger2());
            var mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding>
                {
                    new MessageBinding(typeof(SimpleEvent2), typeof(SimpleEventThrowArgumentExceptionHandler)),
                };
                return binding;
            }).Build();

            bool testChecked = false;
            try
            {
                await mediator.PublishAsync(new SimpleEvent2());
            }
            catch (ArgumentException e)
            {
                e.Message.ShouldBe("aaa");
                testChecked = true;
            }
            testChecked.ShouldBeTrue();
        }
    }
}