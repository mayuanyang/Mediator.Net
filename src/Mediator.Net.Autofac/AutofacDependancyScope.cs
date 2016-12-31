using System;
using Autofac;

namespace Mediator.Net.Autofac
{
    class AutofacDependancyScope : IDependancyScope
    {
        private readonly ILifetimeScope _scope;

        public AutofacDependancyScope(ILifetimeScope scope)
        {
            _scope = scope;
        }
        public void Dispose()
        {
            _scope.Dispose();
        }

        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _scope.Resolve(t);
        }

        public IDependancyScope BeginScope()
        {
            return new AutofacDependancyScope(_scope.BeginLifetimeScope("Mediator.Net"));
        }
    }
}
