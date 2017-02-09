using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.SimpleInjector.Test
{
    class TestBase
    {
        [OneTimeTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
