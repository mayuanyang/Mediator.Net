using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.Middlewares;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPerformance
{
    
    public class SameMediatorSend1000CommandsWithOneMiddleware : TestBase
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
            .Build();

        }

        Task When1000CommandIsSent()
        {
            var allTasks = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
               allTasks.Add(_mediator.SendAsync(new NoWorkCommand()));
            }

            Task.WaitAll(allTasks.ToArray());
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
