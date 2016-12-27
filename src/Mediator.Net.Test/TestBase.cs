using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Test
{
   
    class TestBase
    {
        [TestFixtureTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.Bindings.Clear();
        }
    }
}
