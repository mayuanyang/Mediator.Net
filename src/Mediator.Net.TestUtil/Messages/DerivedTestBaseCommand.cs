using System;

namespace Mediator.Net.TestUtil.Messages
{
    public class DerivedTestBaseCommand : TestBaseCommand
    {
      
        public DerivedTestBaseCommand(Guid id) : base(id)
        {
            Id = id;
        }
    }
}
