using System;
using UnityEngine;

[Serializable]
public class AbilityConfig
{
    [Tooltip("When to give the ability.")]
    public HookType hookType;

    [Tooltip("The ability to give.")]
    public AbilityDefinition abilityDefinition;
}
