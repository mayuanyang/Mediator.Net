using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface ICommandHandler<in TMessage>
        where TMessage : ICommand
    {
        Task Handle(IReceiveContext<TMessage> context, CancellationToken cancellationToken);
    }
    
    public interface ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand where TResponse : IResponse
    {
        Task<TResponse> Handle(IReceiveContext<TCommand> context, CancellationToken cancellationToken);
    }
}
