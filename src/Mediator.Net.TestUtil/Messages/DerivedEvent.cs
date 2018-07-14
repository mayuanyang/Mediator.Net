using System;

namespace Mediator.Net.TestUtil.Messages
{
    public class DerivedEvent : TestEvent
    {
        public string Name { get; set; }

        public DerivedEvent(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}
