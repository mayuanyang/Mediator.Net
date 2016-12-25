using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net
{
    public interface IMediator
    {
        Task SendAsync(ICommand cmd);
        Task PublishAsync(IEvent evt);
    }
}
