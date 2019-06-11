using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class RequestPipeConfigurator : IRequestPipeConfigurator<IReceiveContext<IRequest>>
    {
        private readonly MessageHandlerRegistry _messageHandlerRegistry;
        private readonly IDependencyScope _resolver;
        private readonly IList<IPipeSpecification<IReceiveContext<IRequest>>> _specifications;
        public IDependencyScope DependencyScope => _resolver;
        public RequestPipeConfigurator(MessageHandlerRegistry messageHandlerRegistry, IDependencyScope resolver = null)
        {
            _messageHandlerRegistry = messageHandlerRegistry;
            _resolver = resolver;
            _specifications = new List<IPipeSpecification<IReceiveContext<IRequest>>>();
        }

        public IRequestReceivePipe<IReceiveContext<IRequest>> Build()
        {
            IRequestReceivePipe<IReceiveContext<IRequest>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    current = i == _specifications.Count - 1
                        ? new RequestPipe<IReceiveContext<IRequest>>(_specifications[i], null, _resolver, _messageHandlerRegistry)
                        : new RequestPipe<IReceiveContext<IRequest>>(_specifications[i], current, _resolver, _messageHandlerRegistry);
                }

            }
            else
            {
                current = new RequestPipe<IReceiveContext<IRequest>>(new EmptyPipeSpecification<IReceiveContext<IRequest>>(), null, _resolver, _messageHandlerRegistry);
            }

            return current;
        }

        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<IRequest>> specification)
        {
            _specifications.Add(specification);
        }
    }
}