using System.Collections;
using System.Collections.Generic;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class RequestPipeConfigurator : IRequestPipeConfigurator
    {
        private readonly IList<IPipeSpecification<IReceiveContext<IRequest>>> _specifications;

        public RequestPipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<IReceiveContext<IRequest>>>();
        }


        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<IRequest>> specification)
        {
            _specifications.Add(specification);
        }

        public IRequestPipe<IReceiveContext<IRequest>, IResponse> Build()
        {
            dynamic current = null;
            for (int i = _specifications.Count - 1; i >= 0; i--)
            {
                if (i == _specifications.Count - 1)
                {
                    var thisPipe =
                        new RequestPipe<IReceiveContext<IRequest>, IResponse>(_specifications[i], null);
                    current = thisPipe;
                }
                else
                {
                    var thisPipe =
                        new RequestPipe<IReceiveContext<IRequest>, IResponse>(_specifications[i], current);
                    current = thisPipe;
                }


            }
            return current;
        }

    
    }
}