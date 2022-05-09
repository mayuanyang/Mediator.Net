using System;
using System.Linq;
using System.Reflection;

namespace Mediator.Net;

public class TypeUtil
{
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetTypeInfo().ImplementedInterfaces;

        if (interfaceTypes.Any(it => it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == genericType))
            return true;

        if (givenType.GetTypeInfo().IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        Type baseType = givenType.GetTypeInfo().BaseType;
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }
}