using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class SendPipe<TContext> : ISendPipe<TContext>
        where TContext : IContext<ICommand>
    {
        private readonly IPipeSpecification<TContext> _specification;
   

        public SendPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next)
        {
            Next = next;
            _specification = specification;
        }
  
        public async Task Connect(TContext context)
        {
            await _specification.ExecuteBeforeConnect(context);
            if (Next != null)
            {
                await Next.Connect(context);
            }
          
            await _specification.ExecuteAfterConnect(context);
        }

        public IPipe<TContext> Next { get; internal set; }
    }
}