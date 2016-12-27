using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Test
{
    [TestFixture]
    class TestBase
    {
        [OneTimeTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.Bindings.Clear();
        }
    }
}
