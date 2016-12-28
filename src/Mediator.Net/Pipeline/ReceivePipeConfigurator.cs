using System.Collections;
using System.Collections.Generic;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class ReceivePipeConfigurator : IPipeConfigurator<IContext<IMessage>>
    {
        private readonly IList<IPipeSpecification<IContext<IMessage>>> _specifications;
        public ReceivePipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<IContext<IMessage>>>();
        }


        public void AddPipeSpecification(IPipeSpecification<IContext<IMessage>> specification)
        {
            _specifications.Add(specification);
        }

        public IPipe<IContext<IMessage>> Build()
        {
            dynamic current = null;
            for (int i = _specifications.Count - 1; i >= 0; i--)
            {
                if (i == _specifications.Count - 1)
                {
                    var thisPipe =
                        new ReceivePipe<IContext<IMessage>>(_specifications[i], null);
                    current = thisPipe;
                }
                else
                {
                    var thisPipe =
                        new ReceivePipe<IContext<IMessage>>(_specifications[i], current);
                    current = thisPipe;
                }
                 
                
            }
            return current;
        }
    }
}