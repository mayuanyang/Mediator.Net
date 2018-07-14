using System.Linq;
using Mediator.Net.Binding;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Messages;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestHandlersRegistration
{
    
    public class SameEventMultiHandlerSpec : TestBase
    {
        void GivenAnAssemblyWithMultipleHandlersForTheSameEvent()
        {
            ClearBinding();
        }

        void WhenScanRegistrationIsExecuted()
        {
            var builder = new MediatorBuilder();
            builder.RegisterHandlers(TestUtilAssembly.Assembly).Build();
        }

        void ThenAllHandlersShouldBeRegistered()
        {
            MessageHandlerRegistry.MessageBindings.Count(x => x.MessageType == typeof(TestEvent)).ShouldBe(2);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
