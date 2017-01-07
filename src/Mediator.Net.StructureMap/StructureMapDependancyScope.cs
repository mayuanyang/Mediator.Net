using System;
using StructureMap;

namespace Mediator.Net.StructureMap
{
    public class StructureMapDependancyScope : IDependancyScope
    {
        private readonly IContainer _container;

        public StructureMapDependancyScope(IContainer container)
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

        public IDependancyScope BeginScope()
        {
            var nested = new StructureMapDependancyScope(_container.GetNestedContainer("Mediator.Net"));
            return nested;
        }
    }
}
