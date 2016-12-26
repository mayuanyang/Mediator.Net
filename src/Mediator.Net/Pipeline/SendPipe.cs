using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class SendPipe<TMessage, TContext> : ISendPipe<TMessage, TContext>
        where TMessage : ICommand
        where TContext : IContext<TMessage>
    {
        private readonly IPipe<TMessage, TContext> _pipe;

        public SendPipe(IPipe<TMessage, TContext> next)
        {
            _pipe = next;
        }
  
        public Task Send(TContext context)
        {
            return _pipe.Send(context);
        }
    }
}