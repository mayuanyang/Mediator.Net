using System;
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
    class RequestPipe<TContext> : IRequestReceivePipe<TContext>
        where TContext : IReceiveContext<IRequest>
    {
        private readonly IPipeSpecification<TContext> _specification;
        private readonly IDependencyScope _resolver;

        public RequestPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependencyScope resolver)
        {
            Next = next;
            _specification = specification;
            _resolver = resolver;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
        {
            object result = null;
            try
            {
                await _specification.ExecuteBeforeConnect(context, cancellationToken);
                await _specification.Execute(context, cancellationToken);
                result = await (Next?.Connect(context, cancellationToken) ?? ConnectToHandler(context, cancellationToken));
                await _specification.ExecuteAfterConnect(context, cancellationToken);
            }
            catch (Exception e)
            {
                _specification.OnException(e, context);
            }
            return result;
        }

        private async Task<object> ConnectToHandler(TContext context, CancellationToken cancellationToken)
        {
            var handlers = PipeHelper.GetHandlerBindings(context, true);

            if (handlers.Count() > 1)
            {
                throw new MoreThanOneHandlerException(context.Message.GetType());
            }

            var binding = handlers.Single();

            var handlerType = binding.HandlerType;
            var messageType = context.Message.GetType();

            var handleMethod = handlerType.GetRuntimeMethods().Single(m => PipeHelper.IsHandleMethod(m, messageType));

            var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);

            var task = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });
            await task.ConfigureAwait(false);

            return task.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetValue(task);
        }


        public IPipe<TContext> Next { get; }
    }
}