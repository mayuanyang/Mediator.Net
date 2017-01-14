using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Autofac.Test
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
