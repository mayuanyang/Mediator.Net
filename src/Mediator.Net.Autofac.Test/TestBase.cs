using Mediator.Net.Binding;
using Xunit;

namespace Mediator.Net.Autofac.Test
{
    [Collection("Autofac test")]
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
