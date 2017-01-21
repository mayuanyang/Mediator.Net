using System;

namespace Mediator.Net
{
    public class DependancyScopeNotConfiguredException : Exception
    {
        public DependancyScopeNotConfiguredException(string message) : base(message)
        {
            
        }
    }
}
