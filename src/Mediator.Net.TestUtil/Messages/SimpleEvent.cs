using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class SimpleEvent : IEvent
    {
        public Guid Id { get; }

        public SimpleEvent(Guid id)
        {
            Id = id;
        }
    }
}
