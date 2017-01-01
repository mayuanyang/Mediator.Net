using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        public IReceivePipe<IReceiveContext<IMessage>> ReceivePipe { get; }
        public IRequestPipe<IReceiveContext<IRequest>> RequestPipe { get; }
        public IPublishPipe<IPublishContext<IEvent>> PublishPipe { get; }
        private readonly IDependancyScope _scope;

        public Mediator(IReceivePipe<IReceiveContext<IMessage>> receivePipe,
            IRequestPipe<IReceiveContext<IRequest>> requestPipe, IPublishPipe<IPublishContext<IEvent>> publishPipe, IDependancyScope scope = null)
        {
            ReceivePipe = receivePipe;
            RequestPipe = requestPipe;
            PublishPipe = publishPipe;
            _scope = scope;
        }


        public Task SendAsync<TMessage>(TMessage cmd)
            where TMessage : ICommand
        {
            var task = SendMessage(cmd);
            return task;
        }

        public Task PublishAsync<TMessage>(TMessage evt)
            where TMessage : IEvent
        {
            var task = SendMessage(evt);
            return task;
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var receiveContext = (IReceiveContext<TRequest>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(request.GetType()), request, this);

            var sendMethodInRequestPipe = RequestPipe.GetType().GetMethod("Connect");
            var result = await ((Task<object>)sendMethodInRequestPipe.Invoke(RequestPipe, new object[] { receiveContext })).ConfigureAwait(false);
            
            return (TResponse)result;

        }

        private Task SendMessage<TMessage>(TMessage msg)
            where TMessage : IMessage
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg, this);
            var sendMethodInReceivePipe = ReceivePipe.GetType().GetMethod("Connect");
            var task = (Task)sendMethodInReceivePipe.Invoke(ReceivePipe, new object[] { receiveContext });
            task.ConfigureAwait(false);
            return task;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}