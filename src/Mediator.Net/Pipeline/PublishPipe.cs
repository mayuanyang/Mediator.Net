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

        public async Task<object> Connect(TContext context)
        {
            try
            {
                await _specification.ExecuteBeforeConnect(context);
                if (Next != null)
                {
                    await Next.Connect(context);
                }
                else
                {
                    IMediator mediator;
                    if (context.TryGetService(out mediator))
                    {
                        await mediator.PublishAsync(context.Message);
                    }
                    else
                    {
                        throw new MediatorIsNotAddedToTheContextException();
                    }
                }
                await _specification.ExecuteAfterConnect(context);
            }
            catch (Exception e)
            {
                _specification.OnException(e, context);
            }
            return null;
        }

        public IPipe<TContext> Next { get; }


    }
}