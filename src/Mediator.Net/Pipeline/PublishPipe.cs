using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class PublishPipe<TContext> : IPublishPipe<TContext> where TContext : IPublishContext<IEvent>
    {
        private readonly IPipeSpecification<TContext> _specification;
        private readonly IDependencyScope _resolver;

        public PublishPipe(IPipeSpecification<TContext> specification, IPipe<TContext> next, IDependencyScope resolver = null)
        {
            Next = next;
            _specification = specification;
            _resolver = resolver;
        }

        public async Task<object> Connect(TContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _specification.BeforeExecute(context, cancellationToken).ConfigureAwait(false);
                await _specification.Execute(context, cancellationToken).ConfigureAwait(false);
                if (Next != null)
                {
                    await Next.Connect(context, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    if (context.TryGetService(out IMediator mediator))
                    {
                        await mediator.PublishAsync(context.Message, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new MediatorIsNotAddedToTheContextException();
                    }
                }

                await _specification.AfterExecute(context, cancellationToken).ConfigureAwait(false);
            }
            catch (TargetInvocationException e)
            {
                await _specification.OnException(e, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await _specification.OnException(e, context).ConfigureAwait(false);
            }
            return null;
        }

        public IAsyncEnumerable<TResponse> ConnectStream<TResponse>(TContext context, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Stream is not supported in event pipeline");
        }


        public IPipe<TContext> Next { get; }
    }
}