using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly ICommandReceivePipe<IReceiveContext<ICommand>> _commandReceivePipe;
        private readonly IEventReceivePipe<IReceiveContext<IEvent>> _eventReceivePipe;
        private readonly IRequestReceivePipe<IReceiveContext<IRequest>> _requestPipe;
        private readonly IPublishPipe<IPublishContext<IEvent>> _publishPipe;
        private readonly IGlobalReceivePipe<IReceiveContext<IMessage>> _globalPipe;
        private readonly IDependencyScope _scope;

        public Mediator(
            ICommandReceivePipe<IReceiveContext<ICommand>> commandReceivePipe,
            IEventReceivePipe<IReceiveContext<IEvent>> eventReceivePipe,
            IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe, 
            IPublishPipe<IPublishContext<IEvent>> publishPipe, 
            IGlobalReceivePipe<IReceiveContext<IMessage>> globalPipe, 
            IDependencyScope scope = null)
        {
            _commandReceivePipe = commandReceivePipe;
            _eventReceivePipe = eventReceivePipe;
            _requestPipe = requestPipe;
            _publishPipe = publishPipe;
            _globalPipe = globalPipe;
            _scope = scope;
        }


        public async Task SendAsync<TMessage>(TMessage cmd, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : ICommand
        {
            await SendMessage(cmd, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> SendAsync<TMessage, TResponse>(TMessage cmd, CancellationToken cancellationToken = default(CancellationToken)) where TMessage : ICommand where TResponse : IResponse
        {
            return await SendMessage<TMessage, TResponse>(cmd, cancellationToken).ConfigureAwait(false);
        }

        public async Task SendAsync<TMessage>(IReceiveContext<TMessage> receiveContext,
            CancellationToken cancellationToken = default(CancellationToken))
        where TMessage : ICommand
        {
            await SendMessage(receiveContext, cancellationToken).ConfigureAwait(false);
        }

        public async Task PublishAsync<TMessage>(TMessage evt, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : IEvent
        {
            await SendMessage(evt, cancellationToken).ConfigureAwait(false);
        }

        public async Task PublishAsync<TMessage>(IReceiveContext<TMessage> receiveContext, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : IEvent
        {
            await SendMessage(receiveContext, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default(CancellationToken))
            where TRequest : IRequest
            where TResponse : IResponse
        {
            return await SendMessage<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(IReceiveContext<TRequest> receiveContext, CancellationToken cancellationToken = default(CancellationToken))
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var result = await SendMessage(receiveContext, cancellationToken).ConfigureAwait(false);
            return (TResponse)result;
        }

        public IAsyncEnumerable<TResponse> CreateStream<TRequest, TResponse>(
            IReceiveContext<TRequest> receiveContext,
            CancellationToken cancellationToken = default(CancellationToken)) where TRequest : IMessage where TResponse : IResponse
        {
            return  CreateStreamInternal<TRequest,TResponse>(receiveContext, cancellationToken);
        }

        public IAsyncEnumerable<TResponse> CreateStream<TRequest, TResponse>(TRequest request,
            CancellationToken cancellationToken = default) where TRequest : IMessage where TResponse : IResponse
        {
            return CreateStreamInternal<TRequest,TResponse>(request, cancellationToken);
        }

        private async Task<TResponse> SendMessage<TMessage, TResponse>(TMessage msg, CancellationToken cancellationToken)
            where TMessage : IMessage
        {

            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
            RegisterServiceIfRequired(receiveContext);
            
            receiveContext.ResultDataType = typeof(TResponse);
            
            var task = _globalPipe.Connect((IReceiveContext<IMessage>) receiveContext, cancellationToken);
            
            var result = await task.ConfigureAwait(false);

            return (TResponse)(receiveContext.Result ?? result);
        }
        
        private IAsyncEnumerable<TResponse> CreateStreamInternal<TMessage, TResponse>(IReceiveContext<TMessage> customReceiveContext, [EnumeratorCancellation] CancellationToken cancellationToken)
            where TMessage : IMessage
        {
            RegisterServiceIfRequired(customReceiveContext);

            return _globalPipe.ConnectStream<TResponse>((IReceiveContext<IMessage>)customReceiveContext, cancellationToken);
        }
        
        private IAsyncEnumerable<TResponse> CreateStreamInternal<TMessage, TResponse>(TMessage msg, [EnumeratorCancellation] CancellationToken cancellationToken)
            where TMessage : IMessage
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
            RegisterServiceIfRequired(receiveContext);

            return _globalPipe.ConnectStream<TResponse>((IReceiveContext<IMessage>)receiveContext, cancellationToken);
        }
        
        private async Task<object> SendMessage<TMessage>(TMessage msg, CancellationToken cancellationToken)
            where TMessage : IMessage
        {

            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
            RegisterServiceIfRequired(receiveContext);

            receiveContext.ResultDataType = typeof(object);
            
            var task = _globalPipe.Connect((IReceiveContext<IMessage>) receiveContext, cancellationToken);
            
            var result = await task.ConfigureAwait(false);

            return receiveContext.Result ?? result;
        }

        private async Task<object> SendMessage<TMessage>(IReceiveContext<TMessage> customReceiveContext, CancellationToken cancellationToken)
            where TMessage : IMessage
        {
            RegisterServiceIfRequired(customReceiveContext);

            var task = _globalPipe.Connect((IReceiveContext<IMessage>)customReceiveContext, cancellationToken);
            return await task.ConfigureAwait(false);
        }

        private void RegisterServiceIfRequired<TMessage>(IReceiveContext<TMessage> receiveContext) where  TMessage : IMessage
        {
            receiveContext.RegisterService(this);
            if (!receiveContext.TryGetService(out IPublishPipe<IPublishContext<IEvent>> _))
            {
                receiveContext.RegisterService(_publishPipe);
            }

            if (!receiveContext.TryGetService(out ICommandReceivePipe<IReceiveContext<ICommand>> _))
            {
                receiveContext.RegisterService(_commandReceivePipe);
            }

            if (!receiveContext.TryGetService(out IEventReceivePipe<IReceiveContext<IEvent>> _))
            {
                receiveContext.RegisterService(_eventReceivePipe);
            }

            if (!receiveContext.TryGetService(out IRequestReceivePipe<IReceiveContext<IRequest>> _))
            {
                receiveContext.RegisterService(_requestPipe);
            }
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}