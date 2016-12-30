using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Test.Messages
{
    class GetGuidResponse : IResponse
    {
        public Guid Id { get; }

        public GetGuidResponse(Guid id)
        {
            Id = id;
        }
    }
}
