using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public class ReceiveContext<TMessasge> : IReceiveContext<TMessasge> where TMessasge : IMessage
    {
        public ReceiveContext(TMessasge message)
        {
            Message = message;
        }
        public TMessasge Message { get; }
        public Task Publish(IEvent message)
        {
            throw new System.NotImplementedException();
        }
    }
}