
public enum StatModifierType
{
    Flat,
    PercentAdd,
    PercentMult,
}

public class StatModifier
{
    public StatModifierType Type { get; private set; }
    public float Value { get; private set; }
    public object Source;

    public StatModifier(StatModifierType type, float value, object source)
    {
        Type = type;
        Value = value;
        Source = source;
    }
}