using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class GlobalReceivePipe<TContext> : IGlobalReceivePipe<TContext> where TContext : IContext<IMessage>
    {
        private readonly IPipeSpecification<TContext> _specification;


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
                await _specification.ExecuteBeforeConnect(context, cancellationToken);
                await _specification.Execute(context, cancellationToken);
                if (Next != null)
                {
                    await Next.Connect(context, cancellationToken);
                }
                else
                {
                    result = await ConnectToPipe(context, cancellationToken);
                }

                await _specification.ExecuteAfterConnect(context, cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                _specification.OnException(e, context);
            }
            return result;
        }

        private async Task<object> ConnectToPipe(TContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (context.Message is ICommand)
            {
                if (context.TryGetService(out ICommandReceivePipe<IReceiveContext<ICommand>> commandPipe))
                {
                    await commandPipe.Connect((IReceiveContext<ICommand>)context, cancellationToken);
                }

            }
            else if (context.Message is IEvent)
            {
                if (context.TryGetService(out IEventReceivePipe<IReceiveContext<IEvent>> eventPipe))
                {
                    await eventPipe.Connect((IReceiveContext<IEvent>)context, cancellationToken);
                }
            }
            else if (context.Message is IRequest)
            {
                if (context.TryGetService(out IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe))
                {
                    return await requestPipe.Connect((IReceiveContext<IRequest>)context, cancellationToken);
                }
            }

            return Task.FromResult((object)null);
        }
    }
}