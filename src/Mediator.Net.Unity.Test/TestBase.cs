using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Unity.Test
{
    class TestBase
    {
        [TestFixtureTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
