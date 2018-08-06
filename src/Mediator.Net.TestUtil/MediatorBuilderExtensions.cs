using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mediator.Net.TestUtil.Handlers.CommandHandlers;
using Mediator.Net.TestUtil.Handlers.EventHandlers;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;

namespace Mediator.Net.TestUtil
{
    public static class MediatorBuilderExtensions
    {
        public static MediatorBuilder RegisterUnduplicatedHandlers(this MediatorBuilder mediatorBuilder)
        {
            return mediatorBuilder.RegisterHandlers(
                assembly => assembly.DefinedTypes.Where(t => t.Name != nameof(MultiRequestsHandler) 
                                                             && t.Name != nameof(MultiEventsHandler)
                                                             && t.Name != nameof(MultiCommandsHandler)),
                TestUtilAssembly.Assembly);
        }
    }
}
