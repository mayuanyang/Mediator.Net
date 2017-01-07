using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.IoCTestUtil.Messages
{
    public class SimpleCommand : ICommand
    {
        public Guid Id { get; }

        public SimpleCommand(Guid id)
        {
            Id = id;
        }
    }
}
