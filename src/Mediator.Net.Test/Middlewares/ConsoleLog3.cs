using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.Test.Middlewares
{
    static class ConsoleLog3
    {
        public static void UseConsoleLogger3<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification3<TContext>());
        }
    }

    class ConsoleLogSpecification3<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return true;
        }

        public Task ExecuteBeforeConnect(TContext context, CancellationToken cancellationToken)
        {
            TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
            return Task.FromResult(0);
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
                RubishBox.Rublish.Add(nameof(ConsoleLog3.UseConsoleLogger3));
            }
            
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                TokenRecorder.Recorder.Add(cancellationToken.GetHashCode()); 
            }
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}
