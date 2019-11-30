using Mediator.Net.Binding;
using Mediator.Net.TestUtil.TestUtils;
using Xunit;


namespace Mediator.Net.Test
{

    [Collection("General test")]
    public class TestBase
    {
        public void ClearBinding()
        {
            RubishBox.Rublish.Clear();
            TokenRecorder.Recorder.Clear();
        }
    }
}
