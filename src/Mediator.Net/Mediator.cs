using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IReceivePipe _receivePipe;
        private readonly ISendPipe _sendPipe;

        public Mediator(IReceivePipe receivePipe, ISendPipe sendPipe)
        {
            _receivePipe = receivePipe;
            _sendPipe = sendPipe;
        }
        public Task SendAsync(ICommand cmd)
        {
            return _sendPipe.Send(new SendContext<ICommand>(cmd));
        }

        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}