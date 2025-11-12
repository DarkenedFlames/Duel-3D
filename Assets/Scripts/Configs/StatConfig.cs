using System;
using UnityEngine;

[Serializable]
public class StatConfig
{
    [Tooltip("When to modify the stat.")]
    public HookType hookType;
    
    [Tooltip("The stat to modify.")]
    public StatType statType;

    [Tooltip("If true, modify the 'max' stat, otherwise, modify the 'current' stat.")]
    public bool modifyMax;

    [Tooltip("The amount by which to modify.")]
    public float amount;
}
