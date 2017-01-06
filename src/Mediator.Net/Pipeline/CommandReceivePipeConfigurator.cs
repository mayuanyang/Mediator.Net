using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class CommandReceivePipeConfigurator : ICommandReceivePipeConfigurator
    {
        private readonly IDependancyScope _resolver;
        private readonly IList<IPipeSpecification<IReceiveContext<ICommand>>> _specifications;

        public CommandReceivePipeConfigurator(IDependancyScope resolver = null)
        {
            _resolver = resolver;
            _specifications = new List<IPipeSpecification<IReceiveContext<ICommand>>>();
        }



        public void AddPipeSpecification(IPipeSpecification<IReceiveContext<ICommand>> specification)
        {
            _specifications.Add(specification);
        }

        public ICommandReceivePipe<IReceiveContext<ICommand>> Build()
        {
            ICommandReceivePipe<IReceiveContext<ICommand>> current = null;
            if (_specifications.Any())
            {
                for (int i = _specifications.Count - 1; i >= 0; i--)
                {
                    if (i == _specifications.Count - 1)
                    {
                        var thisPipe =
                            new CommandReceivePipe<IReceiveContext<ICommand>>(_specifications[i], null, _resolver);
                        current = thisPipe;
                    }
                    else
                    {
                        var thisPipe =
                            new CommandReceivePipe<IReceiveContext<ICommand>>(_specifications[i], current, _resolver);
                        current = thisPipe;
                    }


                }
            }
            else
            {
                current = new CommandReceivePipe<IReceiveContext<ICommand>>(new EmptyPipeSpecification<IReceiveContext<ICommand>>(), null, _resolver);
            }

            return current;
        }
    }
}