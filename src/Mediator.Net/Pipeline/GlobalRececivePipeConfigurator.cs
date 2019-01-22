using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class GlobalRececivePipeConfigurator : IGlobalReceivePipeConfigurator
    {
        private readonly IList<IPipeSpecification<IReceiveContext<IMessage>>> _specifications;
        
        public GlobalRececivePipeConfigurator(IDependencyScope dependencyScope)
        {
            DependencyScope = dependencyScope;
            _specifications = new List<IPipeSpecification<IReceiveContext<IMessage>>>();
        }
        public IGlobalReceivePipe<IReceiveContext<IMessage>> Build()
        {
            return Chain();
        }

        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<IMessage>> specification)
        {
            _specifications.Add(specification);
        }

        public IDependencyScope DependencyScope { get; }

        private IGlobalReceivePipe<IReceiveContext<IMessage>> Chain()
        {
            IGlobalReceivePipe<IReceiveContext<IMessage>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    current = i == _specifications.Count - 1 
                        ? new GlobalReceivePipe<IReceiveContext<IMessage>>(_specifications[i], null) 
                        : new GlobalReceivePipe<IReceiveContext<IMessage>>(_specifications[i], current);
                }
                return current;
            }

            return new GlobalReceivePipe<IReceiveContext<IMessage>>(new EmptyPipeSpecification<IReceiveContext<IMessage>>(), null);
        }
    }
}