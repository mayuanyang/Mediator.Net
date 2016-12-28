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
        private readonly IPublishPipe<IPublishContext<IEvent>> _publishPipe;
        private readonly IPipe<IContext<IMessage>> _sendPipe;
        private readonly ConnectionMode _connectionMode;

        public Mediator(IPipe<IContext<IMessage>> receivePipe,
            IPublishPipe<IPublishContext<IEvent>> publishPipe,
            IPipe<IContext<IMessage>> sendPipe,
            ConnectionMode connectionMode)
        {
            _receivePipe = receivePipe;
            _publishPipe = publishPipe;
            _sendPipe = sendPipe;
            _connectionMode = connectionMode;
        }


        public async Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(cmd.GetType()), cmd);
            var sendMethodInReceivePipe = _receivePipe.GetType().GetMethod("Connect");

            if (_sendPipe != null)
            {
                var sendContext =
                    (ISendContext<TMessage>)
                    Activator.CreateInstance(typeof(SendContext<>).MakeGenericType(cmd.GetType()), cmd);
                if (_connectionMode == ConnectionMode.InterConnect)
                {
                    ConnectPipe<IContext<IMessage>>(_sendPipe, _receivePipe);
                    var connectMethodInSendPipe = _sendPipe.GetType().GetMethod("Connect");
                    await (Task)connectMethodInSendPipe.Invoke(_sendPipe, new object[] { sendContext });
                }
                else if (_connectionMode == ConnectionMode.Independant)
                {
                    var connectMethodInSendPipe = _sendPipe.GetType().GetMethod("Connect");
                    await (Task)connectMethodInSendPipe.Invoke(_sendPipe, new object[] { sendContext });
                    await (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
                }
            }
            else
            {
                await (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
            }
        }



        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }

        private void ConnectPipe<TContext>(IPipe<TContext> from,
            IPipe<TContext> to)
            where  TContext : IContext<IMessage>

        {
            if (from.Next != null)
            {
                ConnectPipe(from.Next, to);
            }
            else
            {
                from.GetType().GetProperty("Next").SetValue(from, to);
            }
        }
    }
}