using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestPerformance
{
    class SameMediatorSend1000CommandsOneMiddlewareEachPipe : TestBase
    {
        private IMediator _mediator;
        private long milliSeconds = 0;
        public void GivenAMediator()
        {

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

        public async Task When1000CommandIsSent()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                await _mediator.SendAsync(new NoWorkCommand());
            }

            sw.Stop();
            milliSeconds = sw.ElapsedMilliseconds;
            Console.WriteLine($"it took {milliSeconds} milliseconds");
        }

        public void ThenItShouldNotTakeMoreThan50MilliSeconds()
        {
            milliSeconds.ShouldBeLessThan(50);
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
