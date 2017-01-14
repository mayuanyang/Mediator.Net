using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.StructureMap.Test
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
