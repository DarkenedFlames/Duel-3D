using System;
using UnityEngine;

[Serializable]
public class StatConfig
{
    [Tooltip("When this applies")]
    public HookType hookType;
    [Tooltip("Which stat to modify.")]
    public StatType statType;
    [Tooltip("How much to modify by.")]
    public float amount;
}
