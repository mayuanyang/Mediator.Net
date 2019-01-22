using System;

namespace Mediator.Net
{
    public class DependencyScopeNotConfiguredException : Exception
    {
        public DependencyScopeNotConfiguredException(string message) : base(message)
        {
            
        }
    }
}
