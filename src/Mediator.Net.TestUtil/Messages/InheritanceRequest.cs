using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class InheritanceRequest : IRequest
{
    public Guid Id { get; }

    public string Content { get; set; }

    public InheritanceRequest(Guid id)
    {
        Id = id;
    }
}

public class InheritanceResponse : IResponse
{
    public Guid Id { get; }

    public string Content { get; set; }

    public InheritanceResponse(Guid id)
    {
        Id = id;
    }
}