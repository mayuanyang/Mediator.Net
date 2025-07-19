using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Middlewares;

public static class UselessMiddleware
{
    public static void UseUselessMiddleware<TContext>(this IPipeConfigurator<TContext> configurator)
        where TContext : IContext<IMessage>
    {
        configurator.AddPipeSpecification(new UselessMiddlewareSpecification<TContext>());
    }
}

public class UselessMiddlewareSpecification<TContext> : IPipeSpecification<TContext> 
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return false;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        if (ShouldExecute(context, cancellationToken))
        {
            Console.WriteLine($"you should never see me: {nameof(BeforeExecute)}");
            
            RubishBox.Rublish.Add(nameof(UselessMiddleware.UseUselessMiddleware));
        }

        return Task.FromResult(0);
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        if (ShouldExecute(context, cancellationToken))
            Console.WriteLine($"you should never see me: {nameof(AfterExecute)}");
        
        return Task.FromResult(0);
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        
        throw ex;
    }
}