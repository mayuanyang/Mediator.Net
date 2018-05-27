using Mediator.Net.Binding;

namespace Mediator.Net.SimpleInjector.Test
{
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
