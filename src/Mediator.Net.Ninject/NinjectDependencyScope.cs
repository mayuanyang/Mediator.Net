using System;
using Ninject;

namespace Mediator.Net.Ninject
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private readonly StandardKernel _kernal;

        public NinjectDependencyScope(StandardKernel kernal)
        {
            // TODO: A scope should be used rather than directly using the kernal
            _kernal = kernal;
        }

        public void Dispose()
        {
            // TODO: Waiting for Ninject.Extensions.NamedScope is supported for .NET Core
        }

        public T Resolve<T>()
        {
            return _kernal.Get<T>();
        }

        public object Resolve(Type t)
        {
            return _kernal.GetService(t);
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernal);
        }
    }
}