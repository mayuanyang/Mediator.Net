using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPerformance
{
    public class SameMediatorSend1000CommandsOneMiddlewareEachPipe : TestBase
    {
        private IMediator _mediator;
        private long milliSeconds = 0;
        
        void GivenAMediator()
        {
            ClearBinding();
            
            var builder = new MediatorBuilder();
            
            _mediator = builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(NoWorkCommand), typeof(NoWorkCommandHandler)) };
                
                return binding;
            })
            .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseNoWorkMiddleware();
                })
                .ConfigureCommandReceivePipe(y =>
                {
                    y.UseNoWorkMiddleware();
                })
            .Build();
        }

        void When1000CommandIsSent()
        {
            var sw = new Stopwatch();
            
            sw.Start();
            
            for (int i = 0; i < 1000; i++)
            {
                _mediator.SendAsync(new NoWorkCommand()).Wait();
            }

            sw.Stop();
            
            milliSeconds = sw.ElapsedMilliseconds;
            
            Console.WriteLine($"it took {milliSeconds} milliseconds");
        }

        void ThenItShouldNotTakeMoreThan50MilliSeconds()
        {
            milliSeconds.ShouldBeLessThan(50);
        }

        void AndThenAllCommandShouldGetHandled()
        {
            RubishBox.Rublish.Count.ShouldBe(1000);
        }

        [Trait("Category", "Performance")]
        public void Run()
        {
            this.BDDfy();
        }
    }
}