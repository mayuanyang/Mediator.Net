using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Middlewares;

public static class ConsoleLog2
{
    public static void UseConsoleLogger2<TContext>(this IPipeConfigurator<TContext> configurator)
        where TContext : IContext<IMessage>
    {
        configurator.AddPipeSpecification(new ConsoleLogSpecification2<TContext>());
    }
}

public class ConsoleLogSpecification2<TContext> : IPipeSpecification<TContext> 
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        
        return Task.FromResult(0);
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        if (ShouldExecute(context, cancellationToken))
        {
            TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
            RubishBox.Rublish.Add(nameof(ConsoleLog2.UseConsoleLogger2));
        }
        
        return Task.FromResult(0);
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        if (ShouldExecute(context, cancellationToken))
            TokenRecorder.Recorder.Add(cancellationToken.GetHashCode());
        
        return Task.FromResult(0);
    }

    public Task OnException(Exception ex, TContext context)
    {
        RubishBox.Rublish.Add(ex);
        
        ExceptionDispatchInfo.Capture(ex).Throw();
        
        throw ex;
    }
}