using System;

namespace Mediator.Net;

public interface IDependencyScope : IDisposable
{
    T Resolve<T>();
    
    object Resolve(Type t);
    
    IDependencyScope BeginScope();
}