using System.Collections;
using System.Collections.Generic;
using Mediator.Net.Context;
using Mediator.Net.Contracts;


namespace Mediator.Net.Pipeline
{
    public class ReceivePipeConfigurator<TMessage, TContext> : IPipeConfigurator<TMessage, TContext> 
        where TContext : IReceiveContext<TMessage>
        where TMessage : IMessage
    {
        private readonly IList<IPipeSpecification<TContext, TMessage>> _specifications;
        public ReceivePipeConfigurator()
        {
            _specifications = new List<IPipeSpecification<TContext, TMessage>>();
        }
  

        public void AddPipeSpecification(IPipeSpecification<TContext, TMessage> specification)
        {
            _specifications.Add(specification);
        }

        public IPipe<TContext, TMessage> Build()
        {
            throw new System.NotImplementedException();
        }
    }
}