using Mediator.Net.Binding;
using Mediator.Net.Test.TestUtils;
using NUnit.Framework;

namespace Mediator.Net.Test
{
   
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
            RubishBox.Rublish.Clear();
        }
    }
}
