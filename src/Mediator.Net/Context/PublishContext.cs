using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediator.Net.Contracts;

namespace Mediator.Net.Context
{
    class PublishContext : IPublishContext<IEvent>
    {

        private readonly IList<object> _registeredServices;
        public PublishContext(IEvent message)
        {
            Message = message;
            _registeredServices = new List<object>();
        }
        public IEvent Message { get; }
        public void RegisterService<T>(T service)
        {
            _registeredServices.Add(service);
        }

        public bool TryGetService<T>(out T service)
        {
            var result = _registeredServices.LastOrDefault(x => x.GetType() == typeof(T) || x is T);
            if (result != null)
            {
                service = (T)result;
                return true;
            }
            service = default(T);
            return false;

        }

        
    }
}