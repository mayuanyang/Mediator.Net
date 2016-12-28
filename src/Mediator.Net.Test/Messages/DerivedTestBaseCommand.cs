using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Test.Messages
{
    class DerivedTestBaseCommand : TestBaseCommand
    {
      
        public DerivedTestBaseCommand(Guid id) : base(id)
        {
            Id = id;
        }
    }
}
