using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IReceivePipe<IReceiveContext<IMessage>, IMessage> _receivePipe;
        private readonly ISendPipe<ISendContext<ICommand>, ICommand>  _sendPipe;

        public Mediator(IReceivePipe<IReceiveContext<IMessage>, IMessage>  receivePipe, ISendPipe<ISendContext<ICommand>, ICommand> sendPipe)
        {
            _receivePipe = receivePipe;
            _sendPipe = sendPipe;
        }
  

        public Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(cmd.GetType()), cmd);
            var sendMethod = _receivePipe.GetType().GetMethod("Send");
            //var genericMethod = sendMethod.MakeGenericMethod(receiveContext.GetType());
            return (Task)sendMethod.Invoke(_receivePipe, new object[] { receiveContext } );
        }

    
        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}