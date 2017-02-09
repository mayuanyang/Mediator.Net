using System;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;

namespace Mediator.Net.SimpleInjector
{
    class SimpleInjectorDependancyScope : IDependancyScope
    {
        private Container _container;

        public SimpleInjectorDependancyScope(Container container)
        {
            _container = container;
        }
        public void Dispose()
        {
            
        }

        public T Resolve<T>()  
        {
            return (T) _container.GetInstance(typeof(T));
        }

        public object Resolve(Type t)
        {
            return _container.GetInstance(t);
        }

 
        public IDependancyScope BeginScope()
        {
           throw new NotSupportedException("No inner scope supported");
        }
    }
}
