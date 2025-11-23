using System;

[Serializable]
public class ClampedStat : Stat
{
    public Stat MaxStat;

    public override float Value
    {
        get
        {
            float raw = base.Value;
            if (MaxStat == null) return raw;
            return Math.Clamp(raw, 0f, MaxStat.Value);
        }
    }

    public ClampedStat(StatDefinition definition, Stat maxStat) 
        : base(definition)
    {
        MaxStat = maxStat;
    }
}
