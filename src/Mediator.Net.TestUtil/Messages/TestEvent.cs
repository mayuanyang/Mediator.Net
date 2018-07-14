using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class TestEvent : IEvent
    {
        public Guid Id { get; }

        public TestEvent(Guid id)
        {
            Id = id;
        }
    }
}
