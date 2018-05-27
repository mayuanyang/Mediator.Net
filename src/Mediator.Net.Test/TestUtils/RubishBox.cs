using System;
using System.Collections.Generic;

namespace Mediator.Net.Test.TestUtils
{
    class RubishBox
    {
        [ThreadStatic] static IList<object> _rubish;

        public static IList<object> Rublish => _rubish ?? (_rubish = new List<object>());
    }
}
