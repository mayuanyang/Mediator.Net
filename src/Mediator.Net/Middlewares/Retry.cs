using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Middlewares
{
    static class Retry
    {
        public static void UseRetry<TContext>(this IPipeConfigurator<TContext> configurator, Func<bool> shoudExecute, int numberOfRetry)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new RetrySpecification<TContext>(shoudExecute, numberOfRetry));
        }
    }

    class RetrySpecification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        private int _counter = 1;
        private readonly Func<bool> _shouldExecute;
        private readonly int _numberOfRetry;

        public RetrySpecification(Func<bool> shouldExecute, int numberOfRetry)
        {
            _shouldExecute = shouldExecute;
            _numberOfRetry = numberOfRetry;
        }
        public bool ShouldExecute(TContext context)
        {
            return _shouldExecute();

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            if (_counter >= _numberOfRetry)
            {
                throw ex;
            }
            _counter++;
        }
    }
}
