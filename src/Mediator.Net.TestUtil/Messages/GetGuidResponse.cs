using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class GetGuidResponse : IResponse
{
    public Guid Id { get; }
        
    public string ToBeSetByMiddleware { get; set; }
        
    public int Index { get; set; }

    public GetGuidResponse(Guid id)
    {
        Id = id;
    }
}