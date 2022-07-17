using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.TestUtil.TestUtils;

namespace Mediator.Net.TestUtil.Handlers.CommandHandlers
{
    public abstract class InheritanceBaseCommandHandler : ICommandHandler<InheritanceCommand>
    {
        public abstract Task DoWork(string thing);
        public async Task Handle(IReceiveContext<InheritanceCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);

            await DoWork("From parent");
        }
    }

    public class ChildCommandHandler : InheritanceBaseCommandHandler
    {
        public override async Task DoWork(string thing)
        {
            RubishBox.Rublish.Add(thing);
            await Task.WhenAll();
        }
    }

    public class InheritanceCombinedHandler : ICommandHandler<InheritanceCommand>, ICommandHandler<ChildCommand>
    {
        public Task Handle(IReceiveContext<InheritanceCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);
            return Task.CompletedTask;
        }

        public Task Handle(IReceiveContext<ChildCommand> context, CancellationToken cancellationToken)
        {
            RubishBox.Rublish.Add(context.Message.Id);
            return Task.WhenAll();
        }
    }

    public class InheritanceCombinedWithResponseHandler : ICommandHandler<InheritanceCommand, InheritanceCombinedResponse>, ICommandHandler<ChildCommand, InheritanceCombinedResponse>
    {
        public Task<InheritanceCombinedResponse> Handle(IReceiveContext<InheritanceCommand> context, CancellationToken cancellationToken)
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
}
