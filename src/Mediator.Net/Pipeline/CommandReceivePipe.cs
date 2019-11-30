using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;



namespace Mediator.Net.Pipeline
{
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
            try
            {
                await _specification.BeforeExecute(context, cancellationToken);
                await _specification.Execute(context, cancellationToken);
                await (Next?.Connect(context, cancellationToken) ?? ConnectToHandler(context, cancellationToken));
                await _specification.AfterExecute(context, cancellationToken);
            }
            catch (Exception e)
            {
                await _specification.OnException(e, context);
            }
            return null;
        }

        public IPipe<TContext> Next { get; }

        private async Task ConnectToHandler(TContext context, CancellationToken cancellationToken)
        {
            var handlerBindings = PipeHelper.GetHandlerBindings(context, true, _messageHandlerRegistry);

            if (handlerBindings.Count() > 1)
            {
                throw new MoreThanOneHandlerException(context.Message.GetType());
            }

            var handlerBinging = handlerBindings.Single();
            var handlerType = handlerBinging.HandlerType;
            var messageType = context.Message.GetType();

            var handleMethod = handlerType.GetRuntimeMethods()
                .Single(m => PipeHelper.IsHandleMethod(m, messageType));

            var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);
            var task = (Task)handleMethod.Invoke(handler, new object[] { context, cancellationToken });
            await task.ConfigureAwait(false);

        }
    }
}