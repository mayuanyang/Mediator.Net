using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Autofac.Test
{
    class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
