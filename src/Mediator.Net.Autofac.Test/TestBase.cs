using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using NUnit.Framework;

namespace Mediator.Net.Autofac.Test
{
    class TestBase
    {
        [TestFixtureTearDown]
        public void Teardown()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
        }
    }
}
