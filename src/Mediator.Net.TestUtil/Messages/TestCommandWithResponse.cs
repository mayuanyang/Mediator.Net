using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class TestCommandWithResponse : ICommand
{
    public string Request { get; set; }
    
    public bool ShouldThrow { get; set; } = true;

    public bool ShouldPublishEvent { get; set; }
        
    public bool ShouldEventHandlerThrow { get; set; }
}

public class TestCommandResponse : IResponse
{
    public string Thing { get; set; }
        
}