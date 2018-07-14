using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Mediator.Net.TestUtil
{
    public class TestUtilAssembly
    {
        public static Assembly Assembly => typeof(TestUtilAssembly).GetTypeInfo().Assembly;
    }
}
