using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class PublishPipe<TContext> : IPublishPipe<TContext> where TContext : IPublishContext<IEvent>
    {
        private readonly IPipeSpecification<TContext> _specification;
        private readonly IDependancyScope _resolver;

        public PublishPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependancyScope resolver = null)
        {
            Next = next;
            _specification = specification;
            _resolver = resolver;
        }
        public async Task PublishAsync(TContext context, IMediator mediator)
        {
            await _specification.ExecuteBeforeConnect(context);
            if (Next != null)
            {
                await Next.Connect(context);
            }
            else
            {
                await mediator.PublishAsync(context.Message);
            }

            await _specification.ExecuteAfterConnect(context);
        }

        public Task Connect(TContext context)
        {
            throw new NotImplementedException();
        }

        public IPipe<TContext> Next { get; }

        
    }
}