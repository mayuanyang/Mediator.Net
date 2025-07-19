using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers;

public class ParentAndChildCommandCombinedWithResponseHandler : ICommandHandler<ParentCommand, InheritanceCombinedResponse>, ICommandHandler<ChildCommand, InheritanceCombinedResponse>
{
    public Task<InheritanceCombinedResponse> Handle(IReceiveContext<ParentCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.FromResult<InheritanceCombinedResponse>(new InheritanceCombinedResponse() { Id = context.Message.Id});
    }

    public Task<InheritanceCombinedResponse> Handle(IReceiveContext<ChildCommand> context, CancellationToken cancellationToken)
    {
        RubishBox.Rublish.Add(context.Message.Id);
        
        return Task.FromResult<InheritanceCombinedResponse>(new InheritanceCombinedResponse() { Id = context.Message.Id});
    }
}