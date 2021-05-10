using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages
{
    public class TestCommandWithResponse : ICommand
    {
        
    }

    public class TestCommandResponse : IResponse
    {
        public string Thing { get; set; }
    }
}