using System;
using UnityEngine;

[Serializable]
public class EffectConfig
{
    [Tooltip("When this to modify the effect.")]
    public HookType hookType;

    [Tooltip("Apply if true, remove if false.")]
    public bool mode;

    [Tooltip("The effect to modify.")]
    public EffectDefinition effectDefinition;

    [Tooltip("Number of stacks to apply/remove.")]
    public int stacks;
}
