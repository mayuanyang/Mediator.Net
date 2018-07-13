using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.WebApiSample.Handlers.CommandHandler
{
    public interface ICalculateService
    {
        int Calculate(int left, int right);
    }

    class CalculateService : ICalculateService
    {
        public int Calculate(int left, int right)
        {
            Recorder.Add(left);
            Recorder.Add(right);
            return left + right;
        }
    }

    public class CalculateCommandHandler: ICommandHandler<CalculateCommand>
    {
        private readonly ICalculateService _calculateService;

        public CalculateCommandHandler(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }
        public async Task Handle(ReceiveContext<CalculateCommand> context, CancellationToken cancellationToken)
        {
            var result = _calculateService.Calculate(context.Message.Left, context.Message.Right);
            await context.PublishAsync(new ResultCalculatedEvent(result), CancellationToken.None);
        }
    }
}
