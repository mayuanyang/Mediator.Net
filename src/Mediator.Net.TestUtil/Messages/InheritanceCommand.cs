using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class InheritanceCommand : ICommand
{
    public InheritanceCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
    
public class ParentCommand : ICommand
{
    public ParentCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class ChildCommand : InheritanceCommand
{
    public ChildCommand(Guid id): base(id)
    {
    }
}

public class InheritanceCombinedResponse : IResponse
{
    public Guid Id { get; set; }
}