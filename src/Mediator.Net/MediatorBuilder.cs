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
        
        private bool _isUsingDependancyInjection = false;
        private Action<IReceivePipeConfigurator> _receivePipeConfiguratorAction;
        private Action<IRequestPipeConfigurator<IReceiveContext<IRequest>>> _requestPipeConfiguratorAction;
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
            _receivePipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder ConfigureRequestPipe(Action<IRequestPipeConfigurator<IReceiveContext<IRequest>>> configurator)
        {
            _requestPipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder UseDependancyInjection()
        {
            _isUsingDependancyInjection = true;
            return this;
        }

        public IMediator Build()
        {
            IReceivePipe<IReceiveContext<IMessage>> receivePipe;
            IRequestPipe<IReceiveContext<IRequest>> requestPipe;

            var receivePipeConfigurator = new ReceivePipeConfigurator();
            if (_receivePipeConfiguratorAction == null)
            {
                receivePipe = receivePipeConfigurator.Build();
            }
            else
            {
                _receivePipeConfiguratorAction(receivePipeConfigurator);
                receivePipe = receivePipeConfigurator.Build();
            }
            
            var requestPipeConfigurator = new RequestPipeConfigurator();
            if (_requestPipeConfiguratorAction == null)
            {
                requestPipe = requestPipeConfigurator.Build();
            }
            else
            {
                _requestPipeConfiguratorAction(requestPipeConfigurator);
                requestPipe = requestPipeConfigurator.Build();
            }

            return new Mediator(receivePipe, requestPipe);
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
