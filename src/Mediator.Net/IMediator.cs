using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public interface IMediator : IDisposable
    {
       
        Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand;
        Task PublishAsync<TMessage>(TMessage evt) where TMessage : IEvent;
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest where TResponse : IResponse;
    }
}
