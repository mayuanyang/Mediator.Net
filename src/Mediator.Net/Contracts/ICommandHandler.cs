using System.Threading.Tasks;
using Mediator.Net.Context;

namespace Mediator.Net.Contracts
{
    public interface ICommandHandler<in TMessage> where TMessage : ICommand
    {
        Task Handle(IReceiveContext<TMessage> context);
    }
}
