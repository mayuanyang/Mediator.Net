using Mediator.Net.Binding;
using Xunit;

namespace Mediator.Net.StructureMap.Test
{
    [Collection("StructureMap test")]
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
