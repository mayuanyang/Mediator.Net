using System;
using Mediator.Net.Binding;
using Xunit;

namespace Mediator.Net.MicrosoftDependencyInjection.Test
{

    [Collection("MicrosoftDependencyInjection test")]
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
