using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public class GlobalReceivePipe<TContext> : IGlobalReceivePipe<TContext> where TContext : IContext<IMessage>
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

        public async Task<object> Connect(TContext context)
        {
            object result = null;
            await _specification.ExecuteBeforeConnect(context);
            if (Next != null)
            {
                await Next.Connect(context);
                
            }
            else
            {
                result = await ConnectToPipe(context);
            }

            await _specification.ExecuteAfterConnect(context);
            return result;
        }

        private async Task<object> ConnectToPipe(TContext context)
        {

            if (context.Message is ICommand)
            {
                ICommandReceivePipe<IReceiveContext<ICommand>> commandPipe;
                if (context.TryGetService(out commandPipe))
                {
                    await commandPipe.Connect((IReceiveContext<ICommand>)context);
                }

            }
            else if (context.Message is IEvent)
            {
                IEventReceivePipe<IReceiveContext<IEvent>> eventPipe;
                if (context.TryGetService(out eventPipe))
                {
                    await eventPipe.Connect((IReceiveContext<IEvent>)context);
                }
            }
            else if (context.Message is IRequest)
            {
                IRequestReceivePipe<IReceiveContext<IRequest>> requestPipe;
                if (context.TryGetService(out requestPipe))
                {
                    return await requestPipe.Connect((IReceiveContext<IRequest>)context);
                }
            }

            return Task.FromResult((object)null);
        }
        
    }
}