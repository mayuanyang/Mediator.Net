using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class RequestPipeConfigurator<TContext> : IRequestPipeConfigurator<TContext>
        where TContext : IReceiveContext<IRequest>
    {
        private readonly IList<IPipeSpecification<TContext>> _specifications;

        public RequestPipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<TContext>>();
        }



        public IRequestPipe<TContext> Build()
        {
            IRequestPipe<TContext> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    if (i == _specifications.Count - 1)
                    {
                        var thisPipe =
                            new RequestPipe<TContext>(_specifications[i], null);
                        current = thisPipe;
                    }
                    else
                    {
                        var thisPipe =
                            new RequestPipe<TContext>(_specifications[i], current);
                        current = thisPipe;
                    }


                }
                
            }
            else
            {
                current = new RequestPipe<TContext>(new EmptyPipeSpecification<TContext>(), null);
            }

            return current;
        }


        public void AddPipeSpecification(IPipeSpecification<TContext> specification)
        {
            _specifications.Add(specification);
        }
    }
}