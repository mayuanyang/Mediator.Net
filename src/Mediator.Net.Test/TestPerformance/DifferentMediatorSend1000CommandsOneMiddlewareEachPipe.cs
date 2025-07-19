using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPerformance
{
    public class DifferentMediatorSend1000CommandsOneMiddlewareEachPipe : TestBase
    {
        private long milliSeconds = 0;
        private MediatorBuilder _builder;
        
        void GivenAMediator()
        {
            ClearBinding();
            
            _builder = new MediatorBuilder();
            
            _builder.RegisterHandlers(() =>
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
                });
        }

        Task When1000CommandIsSentByDifferentMediator()
        {
            var allTasks = new List<Task>();
            var sw = new Stopwatch();
            
            sw.Start();
            
            for (int i = 0; i < 1000; i++)
            {
                var mediator = _builder.Build();
                
                allTasks.Add(mediator.SendAsync(new NoWorkCommand()));
            }

            Task.WhenAll(allTasks.ToArray());
            
            sw.Stop();
            
            milliSeconds = sw.ElapsedMilliseconds;
            
            Console.WriteLine($"it took {milliSeconds} milliseconds");
            
            return Task.FromResult(0);
        }

        void ThenItShouldNotTakeMoreThan50MilliSeconds()
        {
            milliSeconds.ShouldBeLessThan(50);
        }

        [Trait("Category", "Performance")]
        public void Run()
        {
            this.BDDfy();
        }
    }
}