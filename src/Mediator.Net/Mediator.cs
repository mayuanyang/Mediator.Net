using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IPipe<IReceiveContext<IMessage>, IMessage> _receivePipe;
        private readonly IPublishPipe<IPublishContext<IEvent>, IEvent> _publishPipe;
        private readonly IPipe<ISendContext<ICommand>, ICommand> _sendPipe;

        public Mediator(IPipe<IReceiveContext<IMessage>, IMessage> receivePipe,
            IPublishPipe<IPublishContext<IEvent>, IEvent> publishPipe,
            IPipe<ISendContext<ICommand>, ICommand> sendPipe)
        {
            _receivePipe = receivePipe;
            _publishPipe = publishPipe;
            _sendPipe = sendPipe;
        }


        public async Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(cmd.GetType()), cmd);
            var sendMethodInReceivePipe = _receivePipe.GetType().GetMethod("Connect");

            if (_sendPipe != null)
            {
                var sendContext = (ISendContext<TMessage>)Activator.CreateInstance(typeof(SendContext<>).MakeGenericType(cmd.GetType()), cmd);
                var connectMethodInSendPipe = _sendPipe.GetType().GetMethod("Connect");
                await (Task)connectMethodInSendPipe.Invoke(_sendPipe, new object[] { sendContext });
            }

            await (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
        }


        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}