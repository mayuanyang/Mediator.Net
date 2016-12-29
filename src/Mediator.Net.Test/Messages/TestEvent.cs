using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Test.Messages
{
    class TestEvent : IEvent
    {
        public Guid Id { get; }

        public TestEvent(Guid id)
        {
            Id = id;
        }
    }
}
