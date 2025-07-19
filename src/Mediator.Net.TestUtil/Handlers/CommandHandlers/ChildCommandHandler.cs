using System.Threading.Tasks;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class ChildCommandHandler : InheritanceBaseCommandHandler
{
    public override async Task DoWork(string thing)
    {
        RubishBox.Rublish.Add(thing);
        
        await Task.WhenAll();
    }
}