using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    public class ReceiveContext<TMessasge> : IReceiveContext<TMessasge>
        where TMessasge : IMessage
    {

        private readonly IList<object> _registeredServices;
        public ReceiveContext(TMessasge message)
        {
            Message = message;
            _registeredServices = new List<object>();
        }
        public TMessasge Message { get; }
        public void RegisterService<T>(T service)
        {
            _registeredServices.Add(service);
        }

        public bool TryGetService<T>(out T service)
        {
            var result = _registeredServices.Last(x => x.GetType() == typeof(T) || x is T);
            if (result != null)
            {
                service = (T)result;
                return true;
            }
            service = default(T);
            return false;

        }

        public Task PublishAsync(IEvent msg)
        {
            IMediator mediator;
            if (TryGetService(out mediator))
            {
                var publishContext = (IPublishContext<IEvent>) Activator.CreateInstance(typeof(PublishContext), msg);
                publishContext.RegisterService(mediator);
                var sendMethod = mediator.PublishPipe.GetType().GetMethod("Connect");
                var task = (Task) sendMethod.Invoke(mediator.PublishPipe, new object[] {publishContext});
                task.ConfigureAwait(false);
                return task;
            }
            throw new MediatorIsNotAddedToTheContextException();
        }
    }
}