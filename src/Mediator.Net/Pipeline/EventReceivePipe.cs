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
        private readonly IDependancyScope _resolver;


        public EventReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependancyScope resolver = null)
        {
            _specification = specification;
            _resolver = resolver;
            Next = next;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _specification.ExecuteBeforeConnect(context);
                await _specification.Execute(context);
                if (Next != null)
                {
                    await Next.Connect(context, cancellationToken);
                }
                else
                {
                    await ConnectToHandler(context, cancellationToken);
                }

                await _specification.ExecuteAfterConnect(context);
            }
            catch (Exception e)
            {
                _specification.OnException(e, context);
            }
            return null;
        }

        public IPipe<TContext> Next { get; }

        private async Task ConnectToHandler(TContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var handlerBindings = PipeHelper.GetHandlerBindings(context, false, cancellationToken);

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