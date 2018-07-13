using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.WebApiSample.Handlers.CommandHandler;

namespace Mediator.Net.WebApiSample.Handlers.EventHandler
{
    public interface IBoardcastService
    {
        void Boardcast(int result);
    }

    class BoardcastService : IBoardcastService
    {
        public void Boardcast(int result)
        {
            Recorder.Add(result);
        }
    }

    public class ResultCalculatedEventHandler: IEventHandler<ResultCalculatedEvent>
    {
        private readonly IBoardcastService _boardcastService;

        public ResultCalculatedEventHandler(IBoardcastService boardcastService)
        {
            _boardcastService = boardcastService;
        }
        public Task Handle(IReceiveContext<ResultCalculatedEvent> context, CancellationToken cancellationToken)
        {
            _boardcastService.Boardcast(context.Message.Result);
            return Task.FromResult(0);
        }
    }
}
