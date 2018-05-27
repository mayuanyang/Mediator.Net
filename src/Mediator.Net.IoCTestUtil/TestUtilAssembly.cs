using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Mediator.Net.IoCTestUtil
{
    public class TestUtilAssembly
    {
        public static Assembly Assembly => typeof(TestUtilAssembly).GetTypeInfo().Assembly;
    }
}
