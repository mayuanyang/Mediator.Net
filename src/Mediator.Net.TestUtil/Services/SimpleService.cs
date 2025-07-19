using System;

namespace Mediator.Net.TestUtil.Services;

public class SimpleService
{
    private readonly AnotherSimpleService _anotherSimpleService;

    public SimpleService(AnotherSimpleService anotherSimpleService)
    {
        _anotherSimpleService = anotherSimpleService;
    }
    
    public void DoWork()
    {
        Console.WriteLine("Job is done");
    }
}