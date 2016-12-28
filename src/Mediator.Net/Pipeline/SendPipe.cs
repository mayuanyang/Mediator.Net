using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class SendPipe<TContext, TMessage> : ISendPipe<TContext, TMessage>
        where TMessage : ICommand
        where TContext : IContext<TMessage>
    {
        private readonly IPipeSpecification<TContext, TMessage> _specification;
        private readonly IPipe<TContext, TMessage> _next;

        public SendPipe(IPipeSpecification<TContext, TMessage> specification, IPipe<TContext, TMessage> next)
        {
            _specification = specification;
            _next = next;
        }
  
        public async Task Connect(TContext context)
        {
            await _specification.ExecuteBeforeConnect(context);
            if (_next != null)
            {
                await _next.Connect(context);
            }
          
            await _specification.ExecuteAfterConnect(context);
        }
    }
}