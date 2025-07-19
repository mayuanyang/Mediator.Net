using System;
using System.Collections.Generic;

namespace Mediator.Net.Autofac.Test
{
    class RubishBin
    {
        [ThreadStatic] static IList<object> _rubish;

        public static IList<object> Rublish => _rubish ??= new List<object>();
    }
}
