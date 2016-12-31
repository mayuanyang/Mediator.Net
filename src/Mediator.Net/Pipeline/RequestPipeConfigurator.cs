using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class RequestPipeConfigurator : IRequestPipeConfigurator<IReceiveContext<IRequest>>
        {
        private readonly IList<IPipeSpecification<IReceiveContext<IRequest>>> _specifications;

        public RequestPipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<IReceiveContext<IRequest>>>();
        }
        
        public IRequestPipe<IReceiveContext<IRequest>> Build()
        {
            IRequestPipe<IReceiveContext<IRequest>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    if (i == _specifications.Count - 1)
                    {
                        var thisPipe =
                            new RequestPipe<IReceiveContext<IRequest>>(_specifications[i], null);
                        current = thisPipe;
                    }
                    else
                    {
                        var thisPipe =
                            new RequestPipe<IReceiveContext<IRequest>>(_specifications[i], current);
                        current = thisPipe;
                    }


                }
                
            }
            else
            {
                current = new RequestPipe<IReceiveContext<IRequest>>(new EmptyPipeSpecification<IReceiveContext<IRequest>>(), null);
            }

            return current;
        }


        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<IRequest>> specification)
        {
            _specifications.Add(specification);
        }
    }
}