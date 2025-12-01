using System;
using System.Collections.Generic;
using UnityEngine;


[Flags] public enum StackingType { None = 0, AddStack = 1 << 0, Refresh = 1 << 1 }

public enum ExpiryType { LoseOneStackAndRefresh, LoseAllStacks }

[CreateAssetMenu]
public class EffectDefinition : ScriptableObject
{
    [Header("Generic Information")]
    [Tooltip("Effect names are currently used for identification.")]
    public string effectName;

    [Tooltip("The icon associated with the effect.")]
    public Sprite icon;

    [Header("Effect Settings")]
    [Tooltip("Determines the process used for this effect's stacking.")]
    public StackingType stackingType = StackingType.Refresh;

    [Tooltip("Determines the process used for this effect's expiration.")]
    public ExpiryType expiryType = ExpiryType.LoseAllStacks;

    [Tooltip("If true, the actions performed by this effect will scale with the number of stacks it has.")]
    public bool ScalesWithStacks = true;

    [Tooltip("The maximum number of stacks this effect can have."), Min(1)]
    public int maxStacks = 1;

    [Tooltip("The lifetime of the effect in seconds (active if greater than 0)."), Min(0)]
    public float duration = 5f;

    [Tooltip("The amount of time between pulses (active if greater than 0)."), Min(0)]
    public float period = 1f;

    [Header("Actions")]
    [Tooltip("The actions that will be executed on the afflicted target upon application."), SerializeReference]
    public List<IGameAction> OnApplyActions;

    [Tooltip("The actions that will be executed on the afflicted target upon pulsing."), SerializeReference]
    public List<IGameAction> OnPulseActions;

    [Tooltip("The actions that will be executed on the afflicted target upon expiration."), SerializeReference]
    public List<IGameAction> OnExpireActions;

}