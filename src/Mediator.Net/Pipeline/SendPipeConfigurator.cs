using System.Collections;
using System.Collections.Generic;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class SendPipeConfigurator : IPipeConfigurator<ISendContext<ICommand>, ICommand>
    {
        private readonly IList<IPipeSpecification<ISendContext<ICommand>, ICommand>> _specifications;
        public SendPipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<ISendContext<ICommand>, ICommand>>();
        }


        public void AddPipeSpecification(IPipeSpecification<ISendContext<ICommand>, ICommand> specification)
        {
            _specifications.Add(specification);
        }

        public IPipe<ISendContext<ICommand>, ICommand> Build()
        {
            dynamic current = null;
            for (int i = _specifications.Count - 1; i >= 0; i--)
            {
                if (i == _specifications.Count - 1)
                {
                    var thisPipe =
                        new SendPipe<ISendContext<ICommand>, ICommand>(
                            (IPipeSpecification<ISendContext<ICommand>, ICommand>) _specifications[i], null);
                    current = thisPipe;
                }
                else
                {
                    var thisPipe =
                        new SendPipe<ISendContext<ICommand>, ICommand>(
                            (IPipeSpecification<ISendContext<ICommand>, ICommand>)_specifications[i], current);
                    current = thisPipe;
                }
                 
                
            }
            return current;
        }
    }
}