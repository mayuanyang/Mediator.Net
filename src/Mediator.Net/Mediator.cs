using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net
{
    public class Mediator : IMediator
    {
        private readonly IPipe<IContext<IMessage>> _receivePipe;
        private readonly IPublishPipe<IPublishContext<IEvent>> _publishPipe;

        public Mediator(IPipe<IContext<IMessage>> receivePipe,
            IPublishPipe<IPublishContext<IEvent>> publishPipe)
        {
            _receivePipe = receivePipe;
            _publishPipe = publishPipe;
        }


        public async Task SendAsync<TMessage>(TMessage cmd) where TMessage : ICommand
        {
            var receiveContext = (IReceiveContext<TMessage>)Activator.CreateInstance(typeof(ReceiveContext<>).MakeGenericType(cmd.GetType()), cmd);
            var sendMethodInReceivePipe = _receivePipe.GetType().GetMethod("Connect");
            await (Task)sendMethodInReceivePipe.Invoke(_receivePipe, new object[] { receiveContext });
            
        }



        public Task PublishAsync(IEvent evt)
        {
            throw new System.NotImplementedException();
        }

        
    }
}