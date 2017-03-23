using Mediator.Net.Binding;

namespace Mediator.Net.Autofac.Test
{
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
