using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class RequestPipe<TContext> : IRequestPipe<TContext>
        where TContext : IReceiveContext<IRequest>
    {
        private readonly IPipeSpecification<TContext> _specification;
        private readonly IDependancyScope _resolver;

        public RequestPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependancyScope resolver)
        {
            Next = next;
            _specification = specification;
            _resolver = resolver;
        }

        public async Task<object> Connect(TContext context)
        {
            object result = null;
            await _specification.ExecuteBeforeConnect(context).ConfigureAwait(false);
            if (Next != null)
            {
                await Next.Connect(context).ConfigureAwait(false);
            }
            else
            {
                result = await ConnectToHandler(context);
            }

            await _specification.ExecuteAfterConnect(context).ConfigureAwait(false);
            return result;
        }

        private Task<object> ConnectToHandler(TContext context)
        {

            var handlers =
                MessageHandlerRegistry.MessageBindings.Where(
                    x => x.MessageType.GetTypeInfo() == context.Message.GetType()).ToList();
            if (!handlers.Any())
                throw new NoHandlerFoundException(context.Message.GetType());

            if (handlers.Count() > 1)
            {
                throw new MoreThanOneHandlerException(context.Message.GetType());
            }

            var binding = handlers.Single();

            var handlerType = binding.HandlerType.GetTypeInfo();
            var messageType = context.Message.GetType();

            var handleMethods = handlerType.GetRuntimeMethods().Where(m => m.Name == "Handle");
            var handleMethod = handleMethods.Single(y =>
            {
                var parameterTypeIsCorrect = y.GetParameters().Single()
                    .ParameterType.GenericTypeArguments.First()
                    .GetTypeInfo()
                    .IsAssignableFrom(messageType.GetTypeInfo());

                return parameterTypeIsCorrect
                       && y.IsPublic
                       && ((y.CallingConvention & CallingConventions.HasThis) != 0);
            });

            var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);
            var task = (Task)handleMethod.Invoke(handler, new object[] { context });

            var taskType = task.GetType();
            var typeInfo = taskType.GetTypeInfo();
            
            var resultProperty = typeInfo.GetDeclaredProperty("Result").GetMethod;
            var result = resultProperty.Invoke(task, new object[] { });

            return Task.FromResult(result);

        }


        Task IPipe<TContext>.Connect(TContext context)
        {
            return Connect(context);
        }

        public IPipe<TContext> Next { get; }
    }
}