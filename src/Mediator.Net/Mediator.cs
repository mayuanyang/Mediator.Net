using System;
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
        private readonly IDependancyScope _scope;

        public Mediator(ICommandReceivePipe<IReceiveContext<ICommand>> commandReceivePipe,
            IEventReceivePipe<IReceiveContext<IEvent>> eventReceivePipe,
            IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe, 
            IPublishPipe<IPublishContext<IEvent>> publishPipe, 
            IGlobalReceivePipe<IReceiveContext<IMessage>> globalPipe, IDependancyScope scope = null)
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
            await SendMessage(cmd, cancellationToken);
        }

        public async Task PublishAsync<TMessage>(TMessage evt, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : IEvent
        {
            await SendMessage(evt, cancellationToken);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default(CancellationToken))
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var result = await SendMessage(request, cancellationToken);
            return (TResponse)result;
        }

        private async Task<object> SendMessage<TMessage>(TMessage msg, CancellationToken cancellationToken)
            where TMessage : IMessage
        {

            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
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

            var task = _globalPipe.Connect((IReceiveContext<IMessage>) receiveContext, cancellationToken);
            return await task.ConfigureAwait(false);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}