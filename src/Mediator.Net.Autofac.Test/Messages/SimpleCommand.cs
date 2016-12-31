using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Autofac.Test.Messages
{
    class SimpleCommand : ICommand
    {
        public Guid Id { get; }

        public SimpleCommand(Guid id)
        {
            Id = id;
        }
    }
}
