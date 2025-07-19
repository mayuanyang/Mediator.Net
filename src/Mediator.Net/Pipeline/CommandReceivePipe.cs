using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline;

public class CommandReceivePipe<TContext> : ICommandReceivePipe<TContext>
    where TContext : IContext<ICommand>
{
    private readonly IPipeSpecification<TContext> _specification;
    private readonly IDependencyScope _resolver;
    private readonly MessageHandlerRegistry _messageHandlerRegistry;

    public CommandReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependencyScope resolver, MessageHandlerRegistry messageHandlerRegistry)
    {
        _specification = specification;
        _resolver = resolver;
        _messageHandlerRegistry = messageHandlerRegistry;
        Next = next;
    }

    public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
    {
        object result = null;
        
        try
        {
            await _specification.BeforeExecute(context, cancellationToken).ConfigureAwait(false);
            await _specification.Execute(context, cancellationToken).ConfigureAwait(false);
            
            result = await 
                (Next?.Connect(context, cancellationToken) ?? 
                 ConnectToHandler(context, cancellationToken)).ConfigureAwait(false);

            context.Result = result as IResponse;
            
            await _specification.AfterExecute(context, cancellationToken).ConfigureAwait(false);
            
        }
        catch (TargetInvocationException e)
        {
            await _specification.OnException(e.InnerException, context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await _specification.OnException(e, context).ConfigureAwait(false);
        }

        return context.Result ?? result;
    }

    public async IAsyncEnumerable<TResponse> ConnectStream<TResponse>(TContext context, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<TResponse> result = null;
        
        try
        {
            await _specification.BeforeExecute(context, cancellationToken).ConfigureAwait(false);
            await _specification.Execute(context, cancellationToken).ConfigureAwait(false);
            
            if (Next != null)
            {
                result = Next.ConnectStream<TResponse>(context, cancellationToken);
            }
            else
            {
                result = ConnectToStreamHandler<TResponse>(context, cancellationToken);
            }

            await _specification.AfterExecute(context, cancellationToken).ConfigureAwait(false);
        }
        catch (TargetInvocationException e)
        {
            await _specification.OnException(e.InnerException, context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await _specification.OnException(e, context).ConfigureAwait(false);
        }

        if (result == null) yield break;
        
        await foreach (var item in result.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return item;
        }
    }

    public IPipe<TContext> Next { get; }

    private async Task<object> ConnectToHandler(TContext context, CancellationToken cancellationToken)
    {
        var handlers = PipeHelper.GetHandlerBindings(context, true, _messageHandlerRegistry);

        if (handlers.Count() > 1)
        {
            throw new MoreThanOneHandlerException(context.Message.GetType());
        }

        var binding = handlers.Single();

        var handlerType = binding.HandlerType;
        var messageType = context.Message.GetType();

        var handleMethod = handlerType.GetRuntimeMethods().Single(m => PipeHelper.IsHandleMethod(m, messageType, false));

        var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);

        if (TypeUtil.IsAssignableToGenericType(handlerType, typeof(IStreamCommandHandler<,>)))
        {
            throw new NotSupportedException(
                "Connecting to a IStreamRequestHandler should use the method of mediator.CreateStream");
        }

        var task = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });
        
        await task.ConfigureAwait(false);

        return PipeHelper.GetResultFromTask(task);
    }
    
    private IAsyncEnumerable<TResponse> ConnectToStreamHandler<TResponse>(TContext context, CancellationToken cancellationToken)
    {
        var handlers = PipeHelper.GetHandlerBindings(context, true, _messageHandlerRegistry);

        if (handlers.Count() > 1)
        {
            throw new MoreThanOneHandlerException(context.Message.GetType());
        }

        var binding = handlers.Single();

        var handlerType = binding.HandlerType;
        var messageType = context.Message.GetType();

        var handleMethod = handlerType.GetRuntimeMethods().Single(m => PipeHelper.IsHandleMethod(m, messageType, false));

        var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);

        var result =  handleMethod.Invoke(handler, new object[] { context, cancellationToken }) as IAsyncEnumerable<TResponse>;

        return result;
    }
}