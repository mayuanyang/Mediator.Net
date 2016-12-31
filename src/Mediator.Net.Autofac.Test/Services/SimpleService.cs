using System;

namespace Mediator.Net.Autofac.Test.Services
{
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
}
