using System;
using Ninject;

namespace Mediator.Net.Ninject
{
    public class NinjectDependancyScope : IDependancyScope
    {
        private readonly StandardKernel _kernal;

        public NinjectDependancyScope(StandardKernel kernal)
        {
            _kernal = kernal;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            return _kernal.Get<T>();
        }

        public object Resolve(Type t)
        {
            return _kernal.GetService(t);
        }

        public IDependancyScope BeginScope()
        {
            return new NinjectDependancyScope(_kernal);
        }
    }
}
