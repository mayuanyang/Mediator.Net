using Mediator.Net.Contracts;

namespace Mediator.Net.WebApiSample.Handlers.CommandHandler
{
    public class ResultCalculatedEvent : IEvent
    {
        public int Result { get; }

        public ResultCalculatedEvent(int result)
        {
            Result = result;
        }
    }
}