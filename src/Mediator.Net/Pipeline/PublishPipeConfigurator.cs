using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    class PublishPipeConfigurator : IPublishPipeConfigurator
    {
        private readonly IDependencyScope _resolver;
        private readonly IList<IPipeSpecification<IPublishContext<IEvent>>> _specifications;
        public IDependencyScope DependencyScope => _resolver;
        public PublishPipeConfigurator(IDependencyScope resolver = null)
        {
            _resolver = resolver;
            _specifications = new List<IPipeSpecification<IPublishContext<IEvent>>>();
        }
        public IPublishPipe<IPublishContext<IEvent>> Build()
        {
            IPublishPipe<IPublishContext<IEvent>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    if (i == _specifications.Count - 1)
                    {
                        var thisPipe =
                            new PublishPipe<IPublishContext<IEvent>>(_specifications[i], null, _resolver);
                        current = thisPipe;
                    }
                    else
                    {
                        var thisPipe =
                            new PublishPipe<IPublishContext<IEvent>>(_specifications[i], current, _resolver);
                        current = thisPipe;
                    }


                }
            }
            else
            {
                current = new PublishPipe<IPublishContext<IEvent>>(new EmptyPipeSpecification<IPublishContext<IEvent>>(), null, _resolver);
            }

            return current;
        }

        public void AddPipeSpecification(IPipeSpecification<IPublishContext<IEvent>> specification)
        {
            _specifications.Add(specification);
        }
    }
}