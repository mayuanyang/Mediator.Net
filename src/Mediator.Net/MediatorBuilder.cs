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
            MessageHandlerRegistry.MessageBindings = messageHandlerPairs;
            return this;
        }

        public MediatorBuilder RegisterHandlers(Func<IList<MessageBinding>> setupBindings)
        {
            var result = setupBindings.Invoke();
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
            IGlobalReceivePipe<IReceiveContext<IMessage>> globalReceivePipe;
            ICommandReceivePipe<IReceiveContext<ICommand>> commandReceivePipe;
            IEventReceivePipe<IReceiveContext<IEvent>> eventReceivePipe;
            IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe;
            IPublishPipe<IPublishContext<IEvent>> publishPipe;

            var commandReceivePipeConfigurator = new CommandReceivePipeConfigurator();
            if (_commandReceivePipeConfiguratorAction == null)
            {
                commandReceivePipe = commandReceivePipeConfigurator.Build();
            }
            else
            {
                _commandReceivePipeConfiguratorAction(commandReceivePipeConfigurator);
                commandReceivePipe = commandReceivePipeConfigurator.Build();
            }

            var eventReceivePipeConfigurator = new EventReceivePipeConfigurator();
            if (_eventReceivePipeConfiguratorAction == null)
            {
                eventReceivePipe = eventReceivePipeConfigurator.Build();
            }
            else
            {
                _eventReceivePipeConfiguratorAction(eventReceivePipeConfigurator);
                eventReceivePipe = eventReceivePipeConfigurator.Build();
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

            var publishPipeConfigurator = new PublishPipeConfigurator();
            if (_publishPipeConfiguratorAction == null)
            {
                publishPipe = publishPipeConfigurator.Build();
            }
            else
            {
                _publishPipeConfiguratorAction(publishPipeConfigurator);
                publishPipe = publishPipeConfigurator.Build();
            }

            var globalPipeConfigurator = new GlobalRececivePipeConfigurator();
            if (_globalReceivePipeConfiguratorAction == null)
            {
                globalReceivePipe = globalPipeConfigurator.Build();
            }
            else
            {
                _globalReceivePipeConfiguratorAction(globalPipeConfigurator);
                globalReceivePipe = globalPipeConfigurator.Build();
            }

            return new Mediator(commandReceivePipe, eventReceivePipe, requestPipe, publishPipe, globalReceivePipe);
        }

        public IMediator Build(IDependancyScope scope)
        {
            IGlobalReceivePipe<IReceiveContext<IMessage>> globalReceivePipe;
            ICommandReceivePipe<IReceiveContext<ICommand>> commandReceivePipe;
            IEventReceivePipe<IReceiveContext<IEvent>> eventReceivePipe;
            IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe;
            IPublishPipe<IPublishContext<IEvent>> publishPipe;

            var receivePipeConfigurator = new CommandReceivePipeConfigurator(scope);
            if (_commandReceivePipeConfiguratorAction == null)
            {
                commandReceivePipe = receivePipeConfigurator.Build();
            }
            else
            {
                _commandReceivePipeConfiguratorAction(receivePipeConfigurator);
                commandReceivePipe = receivePipeConfigurator.Build();
            }

            var eventReceivePipeConfigurator = new EventReceivePipeConfigurator(scope);
            if (_eventReceivePipeConfiguratorAction == null)
            {
                eventReceivePipe = eventReceivePipeConfigurator.Build();
            }
            else
            {
                _eventReceivePipeConfiguratorAction(eventReceivePipeConfigurator);
                eventReceivePipe = eventReceivePipeConfigurator.Build();
            }

            var requestPipeConfigurator = new RequestPipeConfigurator(scope);
            if (_requestPipeConfiguratorAction == null)
            {
                requestPipe = requestPipeConfigurator.Build();
            }
            else
            {
                _requestPipeConfiguratorAction(requestPipeConfigurator);
                requestPipe = requestPipeConfigurator.Build();
            }

            var publishPipeConfigurator = new PublishPipeConfigurator(scope);
            if (_publishPipeConfiguratorAction == null)
            {
                publishPipe = publishPipeConfigurator.Build();
            }
            else
            {
                _publishPipeConfiguratorAction(publishPipeConfigurator);
                publishPipe = publishPipeConfigurator.Build();
            }

            var globalPipeConfigurator = new GlobalRececivePipeConfigurator(scope);
            if (_globalReceivePipeConfiguratorAction == null)
            {
                globalReceivePipe = globalPipeConfigurator.Build();
            }
            else
            {
                _globalReceivePipeConfiguratorAction(globalPipeConfigurator);
                globalReceivePipe = globalPipeConfigurator.Build();
            }
            
            return new Mediator(commandReceivePipe, eventReceivePipe, requestPipe, publishPipe, globalReceivePipe, scope);
        }

        private bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetTypeInfo().ImplementedInterfaces;

            foreach (var it in interfaceTypes)
            {
                if (it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.GetTypeInfo().IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.GetTypeInfo().BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
