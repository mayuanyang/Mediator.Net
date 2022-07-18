using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class GlobalReceivePipe<TContext> : IGlobalReceivePipe<TContext> where TContext : IContext<IMessage>
    {
        private readonly IPipeSpecification<TContext> _specification;


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
                    result = ConnectToStreamPipe<TResponse>(context, cancellationToken);
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

        public GlobalReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next)
        {
            _specification = specification;
            Next = next;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
        {
            object result = null;
            try
            {
                await _specification.BeforeExecute(context, cancellationToken).ConfigureAwait(false);
                await _specification.Execute(context, cancellationToken).ConfigureAwait(false);
                if (Next != null)
                {
                    result = await Next.Connect(context, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    result = await ConnectToPipe(context, cancellationToken).ConfigureAwait(false);
                }

                await _specification.AfterExecute(context, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (TargetInvocationException e)
            {
                var task = _specification.OnException(e.InnerException, context);
                await task.ConfigureAwait(false);
                result = PipeHelper.GetResultFromTask(task);
            }
            catch (Exception e)
            {
                var task = _specification.OnException(e, context);
                await task.ConfigureAwait(false);
                result = PipeHelper.GetResultFromTask(task);
            }
            return context.Result ?? result;
        }

        private async Task<object> ConnectToPipe(TContext context, CancellationToken cancellationToken)
        {
            switch (context.Message)
            {
                case ICommand _:
                {
                    if (context.TryGetService(out ICommandReceivePipe<IReceiveContext<ICommand>> commandPipe))
                    {
                        return await commandPipe.Connect((IReceiveContext<ICommand>)context, cancellationToken).ConfigureAwait(false);
                    }

                    break;
                }

                case IEvent _:
                {
                    if (context.TryGetService(out IEventReceivePipe<IReceiveContext<IEvent>> eventPipe))
                    {
                        await eventPipe.Connect((IReceiveContext<IEvent>)context, cancellationToken).ConfigureAwait(false);
                    }

                    break;
                }

                case IRequest _ when context.TryGetService(out IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe):
                    return await requestPipe.Connect((IReceiveContext<IRequest>)context, cancellationToken).ConfigureAwait(false);
            }

            return (object)null;
        }
        
        private IAsyncEnumerable<TResponse> ConnectToStreamPipe<TResponse>(TContext context, CancellationToken cancellationToken)
        {
            switch (context.Message)
            {
                case ICommand _:
                {
                    if (context.TryGetService(out ICommandReceivePipe<IReceiveContext<ICommand>> commandPipe))
                    {
                        return commandPipe.ConnectStream<TResponse>((IReceiveContext<ICommand>)context, cancellationToken);
                    }

                    break;
                }

                case IEvent _:
                {
                    throw new NotSupportedException("Stream is not supported for IEvent");
                }

                case IRequest _ when context.TryGetService(out IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe):
                    return requestPipe.ConnectStream<TResponse>((IReceiveContext<IRequest>)context, cancellationToken);
            }

            return null;
        }
    }
}