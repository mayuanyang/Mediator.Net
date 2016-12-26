using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class SendPipe<TContext, TMessage> : ISendPipe<TContext, TMessage>
        where TMessage : ICommand
        where TContext : IContext<TMessage>
    {
        private readonly IPipe<TContext, TMessage> _pipe;

        public SendPipe(IPipe<TContext, TMessage> next)
        {
            _pipe = next;
        }
  
        public Task Send(TContext context)
        {
            return _pipe.Send(context);
        }
    }
}