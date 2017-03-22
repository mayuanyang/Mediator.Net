using Mediator.Net.Binding;

namespace Mediator.Net.SimpleInjector.Test
{
    class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
