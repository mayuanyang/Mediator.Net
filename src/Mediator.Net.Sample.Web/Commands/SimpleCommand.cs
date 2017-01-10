using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.Sample.Web.Commands
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