using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IReceivePipe<IMessage, IReceiveContext<IMessage>> _receivePipe;
        private readonly ISendPipe<ICommand, ISendContext<ICommand>>  _sendPipe;

        public Mediator(IReceivePipe<IMessage, IReceiveContext<IMessage>>  receivePipe, ISendPipe<ICommand, ISendContext<ICommand>> sendPipe)
        {
            _receivePipe = receivePipe;
            _sendPipe = sendPipe;
        }
        public Task SendAsync(ICommand cmd)
        {
            return _receivePipe.Send(new ReceiveContext<ICommand>(cmd));
        }

        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}