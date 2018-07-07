using Mediator.Net.Binding;
using Xunit;

namespace Mediator.Net.SimpleInjector.Test
{
    [Collection("SimpleInjector test")]
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
