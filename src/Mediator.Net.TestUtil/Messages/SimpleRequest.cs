using Mediator.Net.Contracts;

namespace Mediator.Net.TestUtil.Messages;

public class SimpleRequest : IRequest
{
    public string Message { get; }

    public SimpleRequest(string message)
    {
        Message = message;
    }
}
    
public class SimpleRequest2 : IRequest
{
}

public class SimpleRequestWillThrow : IRequest
{
    public string Message { get; }

    public SimpleRequestWillThrow(string message)
    {
        Message = message;
    }
}