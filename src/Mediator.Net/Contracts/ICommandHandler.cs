using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface ICommandHandler<TMessage>
        where TMessage : ICommand
    {
        Task Handle(IReceiveContext<TMessage> context, CancellationToken cancellationToken);
    }
}
