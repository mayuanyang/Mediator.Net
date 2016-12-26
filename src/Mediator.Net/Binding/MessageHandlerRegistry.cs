using System;
using System.Collections.Generic;

namespace Mediator.Net.Binding
{
    class MessageHandlerRegistry
    {
        private static IDictionary<Type, Type> _bindings;
        public static IDictionary<Type, Type> Bindings => _bindings ?? (_bindings = new Dictionary<Type, Type>());
    }
}
