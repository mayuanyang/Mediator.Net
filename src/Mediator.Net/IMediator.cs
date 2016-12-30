using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net
{
    public interface IMediator
    {
        Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand;
        Task PublishAsync<TMessage>(TMessage evt) where TMessage : IEvent;
        Task<object> RequestAsync<TRequest>(TRequest request) where TRequest : IRequest;
    }
}
