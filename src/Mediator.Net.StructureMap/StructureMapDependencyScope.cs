using System;
using StructureMap;

namespace Mediator.Net.StructureMap
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private readonly IContainer _container;

        public StructureMapDependencyScope(IContainer container)
        {
            _container = container;
        }
        public void Dispose()
        {
            _container.Dispose();
        }

        public T Resolve<T>()
        {
            return _container.GetInstance<T>();
        }

        public object Resolve(Type t)
        {
            return _container.GetInstance(t);
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(_container.GetNestedContainer("Mediator.Net"));
        }
    }
}
