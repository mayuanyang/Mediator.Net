using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Test.Messages
{
    public class TestBaseCommand : ICommand
    {
        public Guid Id { get; set; }

        public TestBaseCommand(Guid id)
        {
            Id = id;
        }
    }
}
