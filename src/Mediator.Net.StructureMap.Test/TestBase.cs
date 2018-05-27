using Mediator.Net.Binding;

namespace Mediator.Net.StructureMap.Test
{
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
