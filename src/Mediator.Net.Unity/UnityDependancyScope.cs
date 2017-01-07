using System;
using Microsoft.Practices.Unity;

namespace Mediator.Net.Unity
{
    public class UnityDependancyScope : IDependancyScope
    {
        private readonly IUnityContainer _container;

        public UnityDependancyScope(IUnityContainer container)
        {
            _container = container;
        }
        public void Dispose()
        {
            _container.Dispose();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _container.Resolve(t);
        }

        public IDependancyScope BeginScope()
        {
            return new UnityDependancyScope(_container.CreateChildContainer());
        }
    }
}
