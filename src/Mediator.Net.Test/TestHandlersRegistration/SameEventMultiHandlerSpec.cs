using System.Linq;
using Mediator.Net.Binding;
using Mediator.Net.Test.Messages;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test.TestHandlersRegistration
{
    class SameEventMultiHandlerSpec : TestBase
    {
        public void GivenAnAssemblyWithMultipleHandlersForTheSameEvent()
        {
            
        }

        public void WhenScanRegistrationIsExecuted()
        {
            var builder = new MediatorBuilder();
            builder.RegisterHandlers(typeof(SameEventMultiHandlerSpec).Assembly()).Build();
        }

        public void ThenAllHandlersShouldBeRegistered()
        {
            MessageHandlerRegistry.MessageBindings.Count(x => x.MessageType == typeof(TestEvent)).ShouldBe(2);
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
