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

        private Action<IGlobalReceivePipeConfigurator> _globalReceivePipeConfiguratorAction;
        private Action<ICommandReceivePipeConfigurator> _commandReceivePipeConfiguratorAction;
        private Action<IEventReceivePipeConfigurator> _eventReceivePipeConfiguratorAction;
        private Action<IRequestPipeConfigurator<IReceiveContext<IRequest>>> _requestPipeConfiguratorAction;
        private Action<IPublishPipeConfigurator> _publishPipeConfiguratorAction;
        public MediatorBuilder RegisterHandlers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var commandHandlers = assembly.DefinedTypes.Where(x => IsAssignableToGenericType(x.AsType(), typeof(ICommandHandler<>))).ToList();
                foreach (var x in commandHandlers)
                {
                    MessageHandlerRegistry.MessageBindings.Add(new MessageBinding(x.ImplementedInterfaces.First().GenericTypeArguments[0], x.AsType()));
                }
                
                var eventHandlers = assembly.DefinedTypes.Where(x => IsAssignableToGenericType(x.AsType(), typeof(IEventHandler<>))).ToList();
                foreach (var x in eventHandlers)
                {
                    MessageHandlerRegistry.MessageBindings.Add(new MessageBinding(x.ImplementedInterfaces.First().GenericTypeArguments[0], x.AsType()));
                }
                
                var requestHandlers = assembly.DefinedTypes.Where(x => IsAssignableToGenericType(x.AsType(), typeof(IRequestHandler<,>))).ToList();
                foreach (var x in requestHandlers)
                {
                    MessageHandlerRegistry.MessageBindings.Add(new MessageBinding(x.ImplementedInterfaces.First().GenericTypeArguments[0], x.AsType()));
                }
            }
            return this;
        }

        public MediatorBuilder RegisterHandlers(IList<MessageBinding> messageHandlerPairs)
        {
            var messageBindings = new HashSet<MessageBinding>(messageHandlerPairs);
            return RegisterHandlers(messageBindings);
        }

        public MediatorBuilder RegisterHandlers(HashSet<MessageBinding> messageBindings)
        {
            MessageHandlerRegistry.MessageBindings = messageBindings;
            return this;
        }

        public MediatorBuilder RegisterHandlers(Func<IList<MessageBinding>> setupBindings)
        {
            var listBindings = setupBindings();
            return RegisterHandlers(listBindings);
        }

        public MediatorBuilder RegisterHandlers(Func<HashSet<MessageBinding>> setupBindings)
        {
            var result = setupBindings();
            MessageHandlerRegistry.MessageBindings = result;
            return this;
        }

        public MediatorBuilder ConfigureGlobalReceivePipe(Action<IGlobalReceivePipeConfigurator> configurator)
        {
            _globalReceivePipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder ConfigureCommandReceivePipe(Action<ICommandReceivePipeConfigurator> configurator)
        {
            _commandReceivePipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder ConfigureEventReceivePipe(Action<IEventReceivePipeConfigurator> configurator)
        {
            _eventReceivePipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder ConfigureRequestPipe(Action<IRequestPipeConfigurator<IReceiveContext<IRequest>>> configurator)
        {
            _requestPipeConfiguratorAction = configurator;
            return this;
        }

        public MediatorBuilder ConfigurePublishPipe(Action<IPublishPipeConfigurator> configurator)
        {
            _publishPipeConfiguratorAction = configurator;
            return this;
        }


        public IMediator Build()
        {
            return BuildMediator();
        }

        public IMediator Build(IDependencyScope scope)
        {
            return BuildMediator(scope);
        }

        private IMediator BuildMediator(IDependencyScope scope = null)
        {
            var commandReceivePipeConfigurator = new CommandReceivePipeConfigurator(scope);
            _commandReceivePipeConfiguratorAction?.Invoke(commandReceivePipeConfigurator);
            var commandReceivePipe = commandReceivePipeConfigurator.Build();

            var eventReceivePipeConfigurator = new EventReceivePipeConfigurator(scope);
            _eventReceivePipeConfiguratorAction?.Invoke(eventReceivePipeConfigurator);
            var eventReceivePipe = eventReceivePipeConfigurator.Build();


            var requestPipeConfigurator = new RequestPipeConfigurator(scope);
            _requestPipeConfiguratorAction?.Invoke(requestPipeConfigurator);
            var requestPipe = requestPipeConfigurator.Build();

            var publishPipeConfigurator = new PublishPipeConfigurator(scope);
            _publishPipeConfiguratorAction?.Invoke(publishPipeConfigurator);
            var publishPipe = publishPipeConfigurator.Build();

            var globalPipeConfigurator = new GlobalRececivePipeConfigurator(scope);
            _globalReceivePipeConfiguratorAction?.Invoke(globalPipeConfigurator);
            var globalReceivePipe = globalPipeConfigurator.Build();

            return new Mediator(commandReceivePipe, eventReceivePipe, requestPipe, publishPipe, globalReceivePipe, scope);
        }

        private bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetTypeInfo().ImplementedInterfaces;

            if (interfaceTypes.Any(it => it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.GetTypeInfo().IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.GetTypeInfo().BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}
