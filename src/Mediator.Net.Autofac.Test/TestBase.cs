using Mediator.Net.Binding;
using Xunit;

namespace Mediator.Net.Autofac.Test
{

    [Collection("Sequential")]
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
