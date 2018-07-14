using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
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
