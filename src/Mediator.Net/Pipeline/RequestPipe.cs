using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class RequestPipe<TContext, TResponse> : IRequestPipe<TContext, TResponse>
        where TContext : IReceiveContext<IRequest>
        where TResponse : class, IResponse
    {
        private readonly IPipeSpecification<TContext> _specification;

        public RequestPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next)
        {
            Next = next;
            _specification = specification;
        }

        public async Task<TResponse> Connect(TContext context)
        {
            TResponse result = null;
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

        private Task<TResponse> ConnectToHandler(TContext context)
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

            var handler = Activator.CreateInstance(handlerType);
            object result = handleMethod.Invoke(handler, new object[] {context});
            
            try
            {
var a = (Task<IResponse>) result;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            

            return null;

        }


        Task IPipe<TContext>.Connect(TContext context)
        {
            return Connect(context);
        }

        public IPipe<TContext> Next { get; }
    }
}