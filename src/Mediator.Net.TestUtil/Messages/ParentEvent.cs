using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class ParentEvent: IEvent
{
    public Guid Id { get; set; }
}

public class ChildEvent : ParentEvent
{
}