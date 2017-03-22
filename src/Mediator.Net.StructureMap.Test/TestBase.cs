using Mediator.Net.Binding;

namespace Mediator.Net.StructureMap.Test
{
    class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
