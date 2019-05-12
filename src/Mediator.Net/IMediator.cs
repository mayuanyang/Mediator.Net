using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net
{
    public interface IMediator : IDisposable
    {

        Task SendAsync<TMessage>(TMessage cmd, CancellationToken cancellationToken = default(CancellationToken)) where TMessage : ICommand;

        Task SendAsync<TMessage>(IReceiveContext<TMessage> receiveContext,
            CancellationToken cancellationToken = default(CancellationToken)) where TMessage : ICommand;
        Task PublishAsync<TMessage>(TMessage evt, CancellationToken cancellationToken = default(CancellationToken)) where TMessage : IEvent;
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default(CancellationToken)) where TRequest : IRequest where TResponse : IResponse;
    }
}
