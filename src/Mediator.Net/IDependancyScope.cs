using System;

namespace Mediator.Net
{
    public interface IDependancyScope : IDisposable
    {
        T Resolve<T>();
        object Resolve(Type t);
        IDependancyScope BeginScope();
    }
}
