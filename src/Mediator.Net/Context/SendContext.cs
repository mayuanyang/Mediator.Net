using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public class SendContext<TMessage> : ISendContext<TMessage> where TMessage : ICommand
    {
        public SendContext(TMessage message)
        {
            Message = message;
        }
        public TMessage Message { get; }
    }
}