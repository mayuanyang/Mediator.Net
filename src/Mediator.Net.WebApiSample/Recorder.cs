using System.Collections.Generic;

namespace Mediator.Net.WebApiSample;

public static class Recorder
{
    public static List<object> Values = new List<object>();
    
    public static void Add(object value)
    {
        Values.Add(value);
    }
}