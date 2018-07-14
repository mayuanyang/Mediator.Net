using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class AnotherCommand : ICommand
    {
        public Guid Id { get; }

        public AnotherCommand(Guid id)
        {
            Id = id;
        }
    }
}
