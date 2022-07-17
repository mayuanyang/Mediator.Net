using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;



namespace Mediator.Net.Pipeline
{
    public class EventReceivePipe<TContext> : IEventReceivePipe<TContext>
        where TContext : IContext<IEvent>
    {
        private readonly IPipeSpecification<TContext> _specification;
        private readonly IDependencyScope _resolver;
        private readonly MessageHandlerRegistry _messageHandlerRegistry;


        public EventReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependencyScope resolver, MessageHandlerRegistry messageHandlerRegistry)
        {
            _specification = specification;
            _resolver = resolver;
            _messageHandlerRegistry = messageHandlerRegistry;
            Next = next;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _specification.BeforeExecute(context, cancellationToken).ConfigureAwait(false);
                await _specification.Execute(context, cancellationToken).ConfigureAwait(false);
                await (Next?.Connect(context, cancellationToken) ?? ConnectToHandler(context, cancellationToken))
                    .ConfigureAwait(false);
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
            return null;
        }

        public IAsyncEnumerable<TResponse> ConnectStream<TResponse>(TContext context, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Stream is not supported in EventReceivePipe");
        }

        public IPipe<TContext> Next { get; }

        private async Task ConnectToHandler(TContext context, CancellationToken cancellationToken)
        {
            var handlerBindings = PipeHelper.GetHandlerBindings(context, false, _messageHandlerRegistry);

            foreach (var handlerBinding in handlerBindings)
            {
                var handlerType = handlerBinding.HandlerType;
                var messageType = context.Message.GetType();

                var handleMethods = handlerType.GetRuntimeMethods().Where(m => PipeHelper.IsHandleMethod(m, messageType, true));

                foreach (var handleMethod in handleMethods)
                {
                    var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);
                    var task = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });
                    await task.ConfigureAwait(false);    
                }
            }
        }
    }
}