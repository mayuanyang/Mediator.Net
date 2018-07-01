using Mediator.Net.Binding;
using Mediator.Net.Test.TestUtils;


namespace Mediator.Net.Test
{
   
    public class TestBase
    {
        public void ClearBinding()
        {
            MessageHandlerRegistry.MessageBindings.Clear();
            RubishBox.Rublish.Clear();
            TokenRecorder.Recorder.Clear();
        }
    }
}
