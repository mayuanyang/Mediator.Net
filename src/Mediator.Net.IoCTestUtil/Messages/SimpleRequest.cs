using Mediator.Net.Contracts;

namespace Mediator.Net.IoCTestUtil.Messages
{
    public class SimpleRequest : IRequest
    {
        public string Message { get; }

        public SimpleRequest(string message)
        {
            Message = message;
        }
    }
}
