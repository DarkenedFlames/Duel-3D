using System;
using UnityEngine;

[Serializable]
public class StatConfig
{
    [Tooltip("When this applies")]
    public HookType hookType;
    
    [Tooltip("Which stat to modify.")]
    public StatType statType;

    [Tooltip("If true, modify the max stat, otherwise, modify the current stat.")]
    public bool modifyMax;

    [Tooltip("How much to modify by.")]
    public float amount;
}
