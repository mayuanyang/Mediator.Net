using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class InheritanceCommand : ICommand
    {
        public InheritanceCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
