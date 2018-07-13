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


        public EventReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependencyScope resolver = null)
        {
            _specification = specification;
            _resolver = resolver;
            Next = next;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _specification.ExecuteBeforeConnect(context, cancellationToken);
                await _specification.Execute(context, cancellationToken);
                await (Next?.Connect(context, cancellationToken) ?? ConnectToHandler(context, cancellationToken));
                await _specification.ExecuteAfterConnect(context, cancellationToken);
            }
            catch (Exception e)
            {
                _specification.OnException(e, context);
            }
            return null;
        }

        public IPipe<TContext> Next { get; }

        private async Task ConnectToHandler(TContext context, CancellationToken cancellationToken)
        {
            var handlerBindings = PipeHelper.GetHandlerBindings(context, false);

            foreach (var handlerBinding in handlerBindings)
            {
                var handlerType = handlerBinding.HandlerType;
                var messageType = context.Message.GetType();

                var handleMethod = handlerType.GetRuntimeMethods().Single(m => PipeHelper.IsHandleMethod(m, messageType));

                var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);
                var task = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });
                await task.ConfigureAwait(false);
            }
        }
    }
}