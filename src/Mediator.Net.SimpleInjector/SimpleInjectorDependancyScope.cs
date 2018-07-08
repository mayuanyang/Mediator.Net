using System;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;

namespace Mediator.Net.SimpleInjector
{
    class SimpleInjectorDependencyScope : IDependencyScope
    {
        private Container _container;

        public SimpleInjectorDependencyScope(Container container)
        {
            _container = container;
        }
        public void Dispose()
        {
            
        }

        public T Resolve<T>()  
        {
            return (T) Resolve(typeof(T));
        }

        public object Resolve(Type t)
        {
            return _container.GetInstance(t);
        }

 
        public IDependencyScope BeginScope()
        {
           throw new NotSupportedException("No inner scope supported");
        }
    }
}
