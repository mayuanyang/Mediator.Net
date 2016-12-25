using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Pipeline
{
    public class SendPipe : ISendPipe
    {
        private readonly IPipe<ISendContext> _pipe;

        public SendPipe(IPipe<ISendContext> pipe)
        {
            _pipe = pipe;
        }
        public Task Send(ISendContext context)
        {
            return _pipe.Send(context);
        }
    }
}