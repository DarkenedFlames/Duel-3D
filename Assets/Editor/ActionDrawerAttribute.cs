using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ActionDrawerAttribute : Attribute
{
    public Type TargetType { get; }

    public ActionDrawerAttribute(Type targetType)
    {
        TargetType = targetType;
    }
}
