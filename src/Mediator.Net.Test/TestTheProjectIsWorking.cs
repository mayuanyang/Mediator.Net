using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace Mediator.Net.Test
{
    public class TestTheProjectIsWorking
    {
        public void GivenNothing()
        {
            
        }

        public void WhenSomethongHappen()
        {
            
        }

        public void ThenTheTestShouldRun()
        {
            var result = true;
            result.ShouldBe(true);
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
