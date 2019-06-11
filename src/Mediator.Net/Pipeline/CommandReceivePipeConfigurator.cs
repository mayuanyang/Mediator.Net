using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class CommandReceivePipeConfigurator : ICommandReceivePipeConfigurator
    {
        private readonly MessageHandlerRegistry _messageHandlerRegistry;
        private readonly IDependencyScope _resolver;
        private readonly IList<IPipeSpecification<IReceiveContext<ICommand>>> _specifications;

        public IDependencyScope DependencyScope => _resolver;

        public CommandReceivePipeConfigurator(MessageHandlerRegistry messageHandlerRegistry, IDependencyScope resolver = null)
        {
            _messageHandlerRegistry = messageHandlerRegistry;
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
                    current = i == _specifications.Count - 1
                            ? new CommandReceivePipe<IReceiveContext<ICommand>>(_specifications[i], null, _resolver, _messageHandlerRegistry)
                            : new CommandReceivePipe<IReceiveContext<ICommand>>(_specifications[i], current, _resolver, _messageHandlerRegistry);
                }
            }
            else
            {
                current = new CommandReceivePipe<IReceiveContext<ICommand>>(new EmptyPipeSpecification<IReceiveContext<ICommand>>(), null, _resolver, _messageHandlerRegistry);
            }

            return current;
        }
    }
}