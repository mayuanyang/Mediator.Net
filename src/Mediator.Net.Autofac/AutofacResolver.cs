using System;
using Autofac;

namespace Mediator.Net.Autofac
{
    public class AutofacResolver : IResolver
    {
        private readonly ILifetimeScope _container;

        public AutofacResolver(ILifetimeScope container)
        {
            _container = container;
        }
        public T Resolve<T>(Type t)
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _container.Resolve(t);
        }

        public IDependancyScope BeginScope()
        {
            return new AutofacDependancyScope(_container.BeginLifetimeScope("Mediator.Net"));
        }
    }
}
