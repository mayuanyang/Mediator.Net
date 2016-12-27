using System.Collections;
using System.Collections.Generic;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class PipeConfigurator<TContext, TMessage> : IPipeConfigurator<TContext, TMessage>
        where TContext : IReceiveContext<TMessage>
        where TMessage : IMessage
    {
        private readonly IList<IPipeSpecification<TContext, TMessage>> _specifications;
        public PipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<TContext, TMessage>>();
        }


        public void AddPipeSpecification(IPipeSpecification<TContext, TMessage> specification)
        {
            _specifications.Add(specification);
        }

        public IPipe<TContext, TMessage> Build()
        {
            dynamic current = null;
            for (int i = _specifications.Count - 1; i >= 0; i--)
            {
                if (i == _specifications.Count - 1)
                {
                    var thisPipe =
                        new ReceivePipe<IReceiveContext<IMessage>, IMessage>(
                            (IPipeSpecification<IReceiveContext<IMessage>, IMessage>) _specifications[i], null);
                    current = thisPipe;
                }
                else
                {
                    var thisPipe =
                        new ReceivePipe<IReceiveContext<IMessage>, IMessage>(
                            (IPipeSpecification<IReceiveContext<IMessage>, IMessage>)_specifications[i], current);
                    current = thisPipe;
                }
                 
                
            }
            return current;
        }
    }
}