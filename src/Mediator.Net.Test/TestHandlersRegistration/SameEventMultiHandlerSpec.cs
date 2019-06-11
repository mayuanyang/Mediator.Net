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
        private MediatorBuilder _builder;
        void GivenAnAssemblyWithMultipleHandlersForTheSameEvent()
        {
            ClearBinding();
        }

        void WhenScanRegistrationIsExecuted()
        {
            _builder = new MediatorBuilder();
            _builder.RegisterUnduplicatedHandlers().Build();
        }

        void ThenAllHandlersShouldBeRegistered()
        {
            _builder.MessageHandlerRegistry.MessageBindings.Count(x => x.MessageType == typeof(TestEvent)).ShouldBe(2);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
