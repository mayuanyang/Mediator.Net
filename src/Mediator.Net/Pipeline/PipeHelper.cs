using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace Mediator.Net.Pipeline;

public static class PipeHelper
{
    public static List<MessageBinding> GetHandlerBindings<TContext>(TContext context, bool messageTypeExactMatch, MessageHandlerRegistry messageHandlerRegistry) where TContext : IContext<IMessage>
    {
        var handlerBindings = messageTypeExactMatch ?
            messageHandlerRegistry.MessageBindings.Where(x => x.MessageType == context.Message.GetType()).ToList()
            : messageHandlerRegistry.MessageBindings.Where(x => x.MessageType.GetTypeInfo().IsAssignableFrom(context.Message.GetType().GetTypeInfo())).ToList();
        
        if (!handlerBindings.Any())
            throw new NoHandlerFoundException(context.Message.GetType());
        
        return handlerBindings;
    }

    public static bool IsHandleMethod(MethodInfo m, Type messageType, bool isForEvent)
    {
        var exactMatch = m.Name == "Handle" && m.IsPublic && m.GetParameters().Any()
                         && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) ||
                             m.GetParameters()[0].ParameterType.GenericTypeArguments.First().GetTypeInfo()
                                 .Equals(messageType.GetTypeInfo()));
        
        if (!isForEvent) return exactMatch;
        
        if (exactMatch) return true;
        
        return m.Name == "Handle" && m.IsPublic && m.GetParameters().Any()
               && (m.GetParameters()[0].ParameterType.GenericTypeArguments.Contains(messageType) ||
                   m.GetParameters()[0].ParameterType.GenericTypeArguments.First().GetTypeInfo()
                       .IsAssignableFrom(messageType.GetTypeInfo()));
    }

    public static object GetResultFromTask(Task task)
    {
        if (task.GetType().GetRuntimeProperty("Result") == null)
            return null;
        
        if (!task.GetType().GetTypeInfo().IsGenericType)
        {
            throw new Exception("A task without a result is returned");
        }
        
        var result = task.GetType().GetRuntimeProperty("Result").GetMethod;
        
        return result.Invoke(task, new object[] { });
    }
}