using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        private readonly IDependancyScope _resolver;


        public CommandReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependancyScope resolver = null)
        {
            _specification = specification;
            _resolver = resolver;
            Next = next;
        }

        public async Task<object> Connect(TContext context)
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
            return null;
        }

        public IPipe<TContext> Next { get; }

        private Task ConnectToHandler(TContext context)
        {

            var handlers = MessageHandlerRegistry.MessageBindings.Where(x => x.MessageType == context.Message.GetType()).ToList();
            if (!handlers.Any())
                throw new NoHandlerFoundException(context.Message.GetType());

            if (handlers.Count() > 1)
            {
                throw new MoreThanOneHandlerException(context.Message.GetType());
            }


            Task task = null;
            foreach (var x in handlers)
            {
                var handlerType = x.HandlerType;
                var messageType = context.Message.GetType();

                var handleMethod = handlerType.GetRuntimeMethods().Single(m =>
                {
                    var result = m.Name == "Handle" && m.IsPublic && m.GetParameters().Any()
                    && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) || m.GetParameters()[0].ParameterType.GenericTypeArguments.First().GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
                    return result;
                });

                var handler = (_resolver == null) ? Activator.CreateInstance(handlerType) : _resolver.Resolve(handlerType);
                task = (Task)handleMethod.Invoke(handler, new object[] { context });

            }

           
            return task;
        }
    }
}