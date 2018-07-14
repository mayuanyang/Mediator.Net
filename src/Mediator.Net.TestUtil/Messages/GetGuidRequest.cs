using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class GetGuidRequest : IRequest
    {
        public Guid Id { get; }

        public GetGuidRequest(Guid id)
        {
            Id = id;
        }
    }
}
