using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class GetGuidInAsyncRequest : IRequest
    {
        public Guid Id { get; }

        public GetGuidInAsyncRequest(Guid id)
        {
            Id = id;
        }
    }
}