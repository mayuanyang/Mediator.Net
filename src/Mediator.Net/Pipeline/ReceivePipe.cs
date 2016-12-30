using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;



namespace Mediator.Net.Pipeline
{
    public class ReceivePipe<TContext> : IReceivePipe<TContext>
        where TContext : IContext<IMessage>
    {
        private readonly IPipeSpecification<TContext> _specification;


        public ReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next)
        {
            _specification = specification;
            Next = next;
        }

        public async Task Connect(TContext context)
        {
            await _specification.ExecuteBeforeConnect(context);
            if (Next != null)
            {
                await Next.Connect(context);
            }
            else
            {
                await ConnectToHandler(context);
            }

            await _specification.ExecuteAfterConnect(context);
        }

        public IPipe<TContext> Next { get; }

        private async Task ConnectToHandler(TContext context)
        {
            var handlers = MessageHandlerRegistry.MessageBindings.Where(x => x.MessageType.GetTypeInfo() == context.Message.GetType()).ToList();
            if (!handlers.Any())
                throw new NoHandlerFoundException(context.Message.GetType());
            if (typeof(ICommand).IsAssignableFrom(context.GetType().GenericTypeArguments[0]))
            {
                if (handlers.Count() > 1)
                {
                    throw new MoreThanOneHandlerException(context.Message.GetType());
                }
            } 
            
            handlers.ForEach(async x =>
            {
                var handlerType = x.HandlerType.GetTypeInfo();
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
                await (Task)handleMethod.Invoke(handler, new object[] { context });

            });

            await Task.FromResult(0);
        }
    }
}