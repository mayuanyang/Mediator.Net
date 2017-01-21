using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
   
    public interface IPipeConfigurator<TContext>
        where TContext : IContext<IMessage>
    {
        void AddPipeSpecification(IPipeSpecification<TContext> specification);
        IDependancyScope DependancyScope { get; }

    }
}
