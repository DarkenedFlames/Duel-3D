using System;
using UnityEngine;


[Flags]
public enum StackingType
{
    None = 0,
    AddStack = 1 << 0,
    Refresh = 1 << 1
}

public enum ExpiryType
{
    LoseOneStackAndRefresh,
    LoseAllStacks
}

[CreateAssetMenu]
public class EffectDefinition : ScriptableObject
{
    public string effectName;
    public Sprite icon;
    public StackingType stackingType = StackingType.Refresh;
    public ExpiryType expiryType = ExpiryType.LoseAllStacks;
    public int maxStacks = 1;
    public float duration = 5f;
    public float period = 1f;
}