using System;
using System.Reflection;

public static class ObjectExtensions
{
    public static PropertyInfo GetProperty(this object affectedObject, string propertyName)
    {
        var type = affectedObject.GetType();
        var propertyInfo = type.GetProperty(propertyName);
        if (propertyInfo == null)
        {
            throw new Exception(string.Format("Property {0} does not exist in {1}", propertyName, type.Name));
        }
        return propertyInfo;
    }

    public static MethodInfo GetMethod(this object affectedObject, string methodName)
    {
        var type = affectedObject.GetType();
        var methodInfo = type.GetMethod(methodName);
        if (methodInfo == null)
        {
            throw new Exception(string.Format("Method {0} does not exist in {1}", methodName, type.Name));
        }
        return methodInfo;
    }
}