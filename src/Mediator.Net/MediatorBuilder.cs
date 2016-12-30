using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class MediatorBuilder
    {
        private IReceivePipe<IReceiveContext<IMessage>> _receivePipe;
        private IRequestPipe<IReceiveContext<IRequest>> _requestPipe;
        public MediatorBuilder RegisterHandlers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var commandHandlers = assembly.GetTypes().Where(x => IsAssignableToGenericType(x, typeof(ICommandHandler<>))).ToList();
                commandHandlers.ForEach(x =>
                {
                    MessageHandlerRegistry.MessageBindings.Add(new MessageBinding(x.GetInterfaces().First().GenericTypeArguments[0].GetTypeInfo(), x.GetTypeInfo()));
                });
            }
            return this;
        }

        public MediatorBuilder RegisterHandlers(IList<MessageBinding> messageHandlerPairs)
        {
            MessageHandlerRegistry.MessageBindings = messageHandlerPairs;
            return this;
        }

        public MediatorBuilder RegisterHandlers(Func<IList<MessageBinding>> setupBindings)
        {
            var result = setupBindings.Invoke();
            MessageHandlerRegistry.MessageBindings = (IList<MessageBinding>) result;
            return this;
        }

        public MediatorBuilder ConfigureReceivePipe(Action<IReceivePipeConfigurator> configurator)
        {
            var pipeConfigurator = new ReceivePipeConfigurator();
            configurator(pipeConfigurator);
            _receivePipe = pipeConfigurator.Build();
            return this;
        }

        public MediatorBuilder ConfigureRequestPipe<TContext, TResponse>(Action<IRequestPipeConfigurator<TContext>> configurator)
            where TContext : IReceiveContext<IRequest>
        {
            var pipeConfigurator = new RequestPipeConfigurator<TContext>();
            configurator(pipeConfigurator);
            _requestPipe = pipeConfigurator.Build() as IRequestPipe<IReceiveContext<IRequest>>;
            return this;
        }

        public IMediator Build()
        {
            return new Mediator(_receivePipe, _requestPipe);
        }
  
        private bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
