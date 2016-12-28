using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class MediatorBuilder
    {
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

        public ReceivePipeConfigurator BuildPipe(Action<IPipeConfigurator<IContext<IMessage>>> configurator)
        {
            var pipeConfigurator = new ReceivePipeConfigurator();
            configurator.Invoke(pipeConfigurator);
            return pipeConfigurator;
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
