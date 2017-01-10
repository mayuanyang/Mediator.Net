using System;

namespace Mediator.Net.Sample.Web.Services
{
    public interface ISimpleService
    {
        void DoWork();
    }

    class SimpleService : ISimpleService
    {
        public void DoWork()
        {
            
            Console.WriteLine("Job's done");
        }
    }
}