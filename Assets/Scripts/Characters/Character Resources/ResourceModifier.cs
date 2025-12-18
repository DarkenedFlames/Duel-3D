using System;

public enum ResourceModifierType { Increase, Decrease }

public class ResourceModifier
{
    public ResourceModifierType Type { get; private set; }
    public float Value { get; private set; }
    public object Source;

    public ResourceModifier(ResourceModifierType type, float value, object source)
    {
        Type = type;
        Value = value;
        Source = source;
    }
}