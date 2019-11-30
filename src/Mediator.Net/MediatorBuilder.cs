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
        public MessageHandlerRegistry MessageHandlerRegistry {get;}

        public MediatorBuilder()
        {
            MessageHandlerRegistry = new MessageHandlerRegistry();
        }

        public MediatorBuilder RegisterHandlers(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                ScanRegistration(assembly.DefinedTypes);
            }
            return this;
        }

        public MediatorBuilder RegisterHandlers(Func<Assembly, IEnumerable<TypeInfo>> filter, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                ScanRegistration(filter(assembly));
            }

            return this;
        }

        public MediatorBuilder RegisterHandlers(IList<MessageBinding> messageHandlerPairs)
        {
            return RegisterHandlers(new HashSet<MessageBinding>(messageHandlerPairs));
        }

        public MediatorBuilder RegisterHandlers(HashSet<MessageBinding> messageBindings)
        {
            MessageHandlerRegistry.MessageBindings = messageBindings;
            return this;
        }

        public MediatorBuilder RegisterHandlers(Func<IList<MessageBinding>> setupBindings)
        {
            return RegisterHandlers(setupBindings());
        }

        public MediatorBuilder RegisterHandlers(Func<HashSet<MessageBinding>> setupBindings)
        {
            MessageHandlerRegistry.MessageBindings = setupBindings();
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
            var commandReceivePipeConfigurator = new CommandReceivePipeConfigurator(MessageHandlerRegistry, scope);
            _commandReceivePipeConfiguratorAction?.Invoke(commandReceivePipeConfigurator);
            var commandReceivePipe = commandReceivePipeConfigurator.Build();

            var eventReceivePipeConfigurator = new EventReceivePipeConfigurator(MessageHandlerRegistry, scope);
            _eventReceivePipeConfiguratorAction?.Invoke(eventReceivePipeConfigurator);
            var eventReceivePipe = eventReceivePipeConfigurator.Build();


            var requestPipeConfigurator = new RequestPipeConfigurator(MessageHandlerRegistry, scope);
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

        private void ScanRegistration(IEnumerable<TypeInfo> typeInfos)
        {
            var handlers = typeInfos.Where(x => !x.IsAbstract && (IsAssignableToGenericType(x.AsType(), typeof(ICommandHandler<>)) ||
                                                       IsAssignableToGenericType(x.AsType(), typeof(IEventHandler<>)) ||
                                                       IsAssignableToGenericType(x.AsType(), typeof(IRequestHandler<,>)))).ToList();
            foreach (var handler in handlers)
            {
                foreach (var implementedInterface in handler.ImplementedInterfaces)
                {
                    if (IsAssignableToGenericType(implementedInterface, typeof(ICommandHandler<>)) || IsAssignableToGenericType(implementedInterface, typeof(IEventHandler<>)) || IsAssignableToGenericType(implementedInterface, typeof(IRequestHandler<,>)))
                    {
                        MessageHandlerRegistry.MessageBindings.Add(new MessageBinding(implementedInterface.GenericTypeArguments[0], handler.AsType()));
                    } 
                }
            }
        }
    }
}
