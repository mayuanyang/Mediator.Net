using System;
using System.Collections.Generic;

namespace Mediator.Net.TestUtil.TestUtils
{
    public static class TokenRecorder
    {
        [ThreadStatic] static IList<int> _recorder;
        public static IList<int> Recorder => _recorder ?? (_recorder = new List<int>());
    }
}
