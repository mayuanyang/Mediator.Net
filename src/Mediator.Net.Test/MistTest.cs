using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mediator.Net.Test
{
    public class MistTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public MistTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task RunTest()
        {
            await foreach(var dataPoint in FetchIotData())
            {
                _testOutputHelper.WriteLine(dataPoint.ToString());
            }
        }
        
        static async IAsyncEnumerable<int> FetchIotData()
        {
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(1000);//Simulate waiting for data to come through. 
                
                yield return i;
            }
        }
    }
}