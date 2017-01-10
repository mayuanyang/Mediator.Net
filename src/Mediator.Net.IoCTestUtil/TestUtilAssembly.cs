using System.Reflection;

namespace Mediator.Net.IoCTestUtil
{
    public class TestUtilAssembly
    {
        public static Assembly Assembly => typeof(TestUtilAssembly).GetTypeInfo().Assembly;
    }
}
