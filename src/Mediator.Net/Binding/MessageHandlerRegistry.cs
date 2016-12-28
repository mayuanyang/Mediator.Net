using System;
using System.Collections.Generic;

namespace Mediator.Net.Binding
{
    public class MessageHandlerRegistry
    {
        private static IDictionary<Type, Type> _bindings;

        public static IDictionary<Type, Type> Bindings
        {
            get
            {
                return _bindings ?? (_bindings = new Dictionary<Type, Type>());
            }
            internal set { _bindings = value; }
        } 
        
    }
}
