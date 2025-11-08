using System;
using UnityEngine;

[Serializable]
public class DamageConfig
{
    [Tooltip("When this damage applies (e.g., OnApply, OnTick, OnExpire)")]
    public HookType hookType;

    [Tooltip("How much damage to deal (per second if OnTick).")]
    public float amount;
}
