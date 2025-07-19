﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Middlewares;

public static class MiddlewareThrowExAfterConnect
{
    public static void UseMiddlewareThrowExAfterConnect<TContext>(this IPipeConfigurator<TContext> configurator)
        where TContext : IContext<IMessage>
    {
        configurator.AddPipeSpecification(new MiddlewareThrowExAfterConnectSpecification<TContext>());
    }
}

public class MiddlewareThrowExAfterConnectSpecification<TContext> : IPipeSpecification<TContext> 
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        throw new Exception();
    }

    public Task OnException(Exception ex, TContext context)
    {
        RubishBox.Rublish.Add(ex);
        
        ExceptionDispatchInfo.Capture(ex).Throw();
        
        throw ex;
    }
}