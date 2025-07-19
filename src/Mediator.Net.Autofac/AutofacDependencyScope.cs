using System;
using Autofac;

namespace Mediator.Net.Autofac
{
    class AutofacDependencyScope : IDependencyScope
    {
        private readonly ILifetimeScope _scope;

        public AutofacDependencyScope(ILifetimeScope scope)
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

        public IDependencyScope BeginScope()
        {
            return new AutofacDependencyScope(_scope.BeginLifetimeScope());
        }
    }
}