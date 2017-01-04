using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class GlobalReceivePipe<TContext> : IGlobalReceivePipe<TContext> where TContext : IContext<IMessage>
    {
        private IPipeSpecification<TContext> _specification;
        private IDependancyScope _resolver;

        public IPipe<TContext> Next { get; }

        public GlobalReceivePipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependancyScope resolver = null)
        {
            _specification = specification;
            _resolver = resolver;
            Next = next;
        }

        public async Task Connect(TContext context)
        {
            await _specification.ExecuteBeforeConnect(context);
            if (Next != null)
            {
                await Next.Connect(context);
            }
            else
            {
                await ConnectToPipe(context);
            }

            await _specification.ExecuteAfterConnect(context);
        }

        private async Task ConnectToPipe(TContext context)
        {
            if (context.Message is ICommand)
            {
                IReceivePipe<IReceiveContext<ICommand>> commandPipe;
                if (context.TryGetService(out commandPipe))
                {
                    await commandPipe.Connect((IReceiveContext<ICommand>)context);
                }

            }
            else if (context.Message is IEvent)
            {
                IReceivePipe<IReceiveContext<IEvent>> commandPipe;
                if (context.TryGetService(out commandPipe))
                {
                    await commandPipe.Connect((IReceiveContext<IEvent>)context);
                }
            }
            else if (context.Message is IRequest)
            {
                IRequestPipe<IReceiveContext<IRequest>> commandPipe;
                if (context.TryGetService(out commandPipe))
                {
                    await commandPipe.Connect((IReceiveContext<IRequest>)context);
                }
            }

        }
        
    }
}