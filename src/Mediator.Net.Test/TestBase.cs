using Mediator.Net.Binding;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;

namespace Mediator.Net.Test
{
   
    class TestBase
    {
        [OneTimeTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
            RubishBox.Rublish.Clear();
        }
    }
}
