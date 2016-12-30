using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Test.Messages
{
    class GetGuidRequest : IRequest
    {
        public Guid Id { get; }

        public GetGuidRequest(Guid id)
        {
            Id = id;
        }
    }
}
