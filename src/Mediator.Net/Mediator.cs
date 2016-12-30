using System;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IReceivePipe<IReceiveContext<IMessage>> _receivePipe;
        private readonly IRequestPipe<IReceiveContext<IRequest>> _requestPipe;

        public Mediator(IReceivePipe<IReceiveContext<IMessage>> receivePipe,
            IRequestPipe<IReceiveContext<IRequest>> requestPipe)
        {
            _receivePipe = receivePipe;
            _requestPipe = requestPipe;
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

        public Task<object> RequestAsync<TRequest>(TRequest request)
            where TRequest : IRequest

        {
            var receiveContext =
                (IReceiveContext<TRequest>)
                Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(request.GetType()), request);

            var sendMethodInRequestPipe = _requestPipe.GetType().GetMethod("Connect");
            var value = (Task<object>)sendMethodInRequestPipe.Invoke(_requestPipe, new object[] { receiveContext });
            // task.ConfigureAwait(false);
            return value;

        }

        private Task SendMessage<TMessage>(TMessage msg)
            where TMessage : IMessage
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
            var sendMethodInReceivePipe = _receivePipe.GetType().GetMethod("Connect");
            var task = (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
            task.ConfigureAwait(false);
            return task;
        }
    }
}