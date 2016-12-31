using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class ReceivePipeConfigurator : IReceivePipeConfigurator
    {
        private readonly IDependancyScope _resolver;
        private readonly IList<IPipeSpecification<IReceiveContext<IMessage>>> _specifications;

        public ReceivePipeConfigurator(IDependancyScope resolver = null)
        {
            _resolver = resolver;
            _specifications = new List<IPipeSpecification<IReceiveContext<IMessage>>>();
        }



        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<IMessage>> specification)
        {
            _specifications.Add(specification);
        }

        public IReceivePipe<IReceiveContext<IMessage>> Build()
        {
            IReceivePipe<IReceiveContext<IMessage>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    if (i == _specifications.Count - 1)
                    {
                        var thisPipe =
                            new ReceivePipe<IReceiveContext<IMessage>>(_specifications[i], null, _resolver);
                        current = thisPipe;
                    }
                    else
                    {
                        var thisPipe =
                            new ReceivePipe<IReceiveContext<IMessage>>(_specifications[i], current, _resolver);
                        current = thisPipe;
                    }


                }
            }
            else
            {
                current = new ReceivePipe<IReceiveContext<IMessage>>(new EmptyPipeSpecification<IReceiveContext<IMessage>>(), null, _resolver);
            }

            return current;
        }
    }
}