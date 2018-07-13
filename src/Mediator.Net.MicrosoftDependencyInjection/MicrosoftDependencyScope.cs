using System;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Net.MicrosoftDependencyInjection
{
    public class MicrosoftDependencyScope : IDependencyScope
    {
        private readonly IServiceScope _scope;

        public MicrosoftDependencyScope(IServiceScope scope)
        {
            _scope = scope;
        }
        public void Dispose()
        {
            _scope.Dispose();
        }

        public T Resolve<T>()
        {
            return _scope.ServiceProvider.GetService<T>();
        }

        public object Resolve(Type t)
        {
            return _scope.ServiceProvider.GetService(t);
        }

        public IDependencyScope BeginScope()
        {
            return new MicrosoftDependencyScope(_scope.ServiceProvider.CreateScope());
        }
    }
}
