using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline
{
    public static class PipeHelper
    {
        public static List<MessageBinding> GetHandlerBindings<TContext>(TContext context, bool messageTypeExactMatch, CancellationToken cancellationToken) where TContext : IContext<IMessage>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var handlerBindings = messageTypeExactMatch ?
                MessageHandlerRegistry.MessageBindings.Where(x => x.MessageType == context.Message.GetType()).ToList()
                : MessageHandlerRegistry.MessageBindings.Where(x => x.MessageType.GetTypeInfo().IsAssignableFrom(context.Message.GetType().GetTypeInfo())).ToList();
            if (!handlerBindings.Any())
                throw new NoHandlerFoundException(context.Message.GetType());
            return handlerBindings;
        }

        public static bool IsHandleMethod(MethodInfo m, Type messageType)
        {
            return m.Name == "Handle" && m.IsPublic && m.GetParameters().Any()
                         && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) || m.GetParameters()[0].ParameterType.GenericTypeArguments.First().GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
        }
    }
}
