using System;
using UnityEngine;

[Serializable]
public class EffectConfig
{
    [Tooltip("When this applies")]
    public HookType hookType;

    [Tooltip("Apply if true, remove if false")]
    public bool mode;

    [Tooltip("Effect to modify")]
    public EffectDefinition effectDefinition;

    [Tooltip("Number of stacks to apply/remove.")]
    public int stacks;
}
