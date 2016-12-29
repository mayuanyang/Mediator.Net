using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IPipe<IContext<IMessage>> _receivePipe;
        private readonly IPublishPipe<IPublishContext> _publishPipe;

        public Mediator(IPipe<IContext<IMessage>> receivePipe,
            IPublishPipe<IPublishContext> publishPipe)
        {
            _receivePipe = receivePipe;
            _publishPipe = publishPipe;
        }


        public async Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand
        {
            await SendMessage(cmd);
        }



        public async Task PublishAsync<TMessage>(TMessage evt) where TMessage : IEvent
        {
            await SendMessage(evt);
        }

        private async Task SendMessage<TMessage>(TMessage msg) where TMessage : IMessage
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(msg.GetType()), msg);
            var sendMethodInReceivePipe = _receivePipe.GetType().GetMethod("Connect");
            await (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
        }

        
    }
}