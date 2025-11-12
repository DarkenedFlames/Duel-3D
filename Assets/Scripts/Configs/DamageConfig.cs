using System;
using UnityEngine;

[Serializable]
public class DamageConfig
{
    [Tooltip("When to deal damage.")]
    public HookType hookType;

    [Tooltip("The amount of damage to deal.")]
    public float amount;
}
